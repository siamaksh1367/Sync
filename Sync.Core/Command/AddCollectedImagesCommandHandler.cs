using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Sync.Core.Infrustructure;
using Sync.DAL;
using Sync.DAL.Models;
using Sync.Services.FieldSatClient;
using Sync.Services.MultipleProviders;
using Sync.Services.MultipleProviders.TimeProviders;
using Image = Sync.DAL.Models.Image;

namespace Sync.Core.Command
{
    public class AddCollectedImageCommandHandler(
        IEnumerable<IDataProvider<IEnumerable<Guid>>> fieldProviders,
        IEnumerable<IDataProvider<TimePeriodObject>> periodProviders,
        IUnitOfWork unitOfWork,
        IImageSatClient imageSatClient,
        IMapper mapper,
        IQueryStringBuilder queryStringBuilder,
        ILogger<AddCollectedImageCommandHandler> logger) : ICommandHandler<AddCollectedImagesCommand, Unit>
    {
        private readonly IEnumerable<IDataProvider<IEnumerable<Guid>>> _fieldProviders = fieldProviders;
        private readonly IEnumerable<IDataProvider<TimePeriodObject>> _periodProviders = periodProviders;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IImageSatClient _imageSatClient = imageSatClient;
        private readonly IMapper _mapper = mapper;
        private readonly IQueryStringBuilder _queryStringBuilder = queryStringBuilder;
        private readonly ILogger<AddCollectedImageCommandHandler> _logger = logger;
        private List<TimePeriod> _toDelete;
        private List<TimePeriodObject> _toCollect;
        private TimePeriod? _toleft;
        private TimePeriod? _toRight;
        private bool _allowCachc = false;

        public async Task<Unit> Handle(AddCollectedImagesCommand request, CancellationToken cancellationToken)
        {
            var fields = await GetListOfFields(request);
            var inputPeriod = await GetTimePeriods(request);
            var newTimePeriod = new TimePeriod()
            {
                Id = Guid.NewGuid(),
                Fields = new List<Field>()
            };
            foreach (var field in fields)
            {
                var existingPeriods = await _unitOfWork.TimePeriodRepository.GetByFieldId(field);
                FilterDates(inputPeriod, existingPeriods, ref newTimePeriod);
                if (!_toCollect.IsNullOrEmpty())
                {
                    foreach (var toCollect in _toCollect)
                    {
                        var timePeriodDictionary = _queryStringBuilder.SetQueryString(toCollect.StartDate, toCollect.EndDate);
                        var images = await _imageSatClient.GetImagesAsync(field, timePeriodDictionary);
                        if (!images.IsNullOrEmpty())
                        {
                            foreach (var image in images)
                            {
                                await _unitOfWork.ImageRepository.Add(_mapper.Map<Image>(image, opt => opt.Items["field"] = field));
                            }
                            var fieldFromDb = await _unitOfWork.FieldRepository.GetById(field);
                            if (!newTimePeriod.Fields.Contains(fieldFromDb))
                                newTimePeriod.Fields.Add(fieldFromDb);
                        }
                    }
                    _toCollect = new List<TimePeriodObject>();
                }
            }
            if (!_toDelete.IsNullOrEmpty())
            {
                BringMissingFields(newTimePeriod);
                DeleteMergedTmePeriod();
            }
            await AddNewTimePeriod(newTimePeriod);
            await _unitOfWork.SaveAsync();
            return Unit.Value;
        }

        private async Task AddNewTimePeriod(TimePeriod newTimePeriod)
        {
            if (_allowCachc && !newTimePeriod.Fields.IsNullOrEmpty())
                await _unitOfWork.TimePeriodRepository.Add(newTimePeriod);
        }

        private void DeleteMergedTmePeriod()
        {
            foreach (var toDeletetem in _toDelete)
                _unitOfWork.TimePeriodRepository.Delete(toDeletetem);
        }

        private void BringMissingFields(TimePeriod newTimePeriod)
        {
            foreach (var field in _toDelete.SelectMany(toDelete => toDelete.Fields.Where(field => !newTimePeriod.Fields.Contains(field))))
            {
                newTimePeriod.Fields.Add(field);
            }
        }

        private void FilterDates(TimePeriodObject date, IEnumerable<TimePeriod> timePeriods, ref TimePeriod newTimePeriod)
        {
            if (IfCached(date, timePeriods))
            {
                _allowCachc = false;
                return;
            }

            if (IfNotCached(date, timePeriods))
            {
                _allowCachc = true;
                newTimePeriod.StartDate = date.StartDate;
                newTimePeriod.EndDate = date.EndDate;
                return;
            }

            if (IfPartlyCached(date, timePeriods))
            {
                _allowCachc = true;
                newTimePeriod.StartDate = date.StartDate;
                newTimePeriod.EndDate = date.EndDate;
            }

            if (IfHasLeftCached(date, timePeriods))
            {
                _allowCachc = true;
                newTimePeriod.StartDate = _toleft.StartDate;
            }

            if (IfHasRightCached(date, timePeriods))
            {
                _allowCachc = true;
                newTimePeriod.EndDate = _toRight.EndDate;
            }
        }

        private bool IfHasRightCached(TimePeriodObject date, IEnumerable<TimePeriod> timePeriods)
        {
            _toRight = timePeriods.FirstOrDefault(x => x.EndDate > date.EndDate && x.StartDate < date.EndDate);
            if (_toRight is not null)
            {
                _toDelete.Add(_toRight);
                _toCollect[_toCollect.Count() - 1] = new TimePeriodObject(_toCollect[_toCollect.Count() - 1].StartDate, _toRight.StartDate.AddDays(-1));
                return true;
            }
            return false;
        }

        private bool IfHasLeftCached(TimePeriodObject date, IEnumerable<TimePeriod> timePeriods)
        {
            _toleft = timePeriods.FirstOrDefault(x => x.StartDate < date.StartDate && x.EndDate > date.StartDate);
            if (_toleft is not null)
            {
                _toDelete.Add(_toleft);
                _toCollect[0] = new TimePeriodObject(_toleft.EndDate.AddDays(1), _toCollect[0].EndDate);
                return true;
            }
            return false;
        }

        private bool IfPartlyCached(TimePeriodObject date, IEnumerable<TimePeriod> timePeriods)
        {
            _toDelete = timePeriods.Where(x => x.StartDate >= date.StartDate && x.EndDate <= date.EndDate).OrderBy(x => x.StartDate).ToList();
            if (!_toDelete.IsNullOrEmpty())
            {
                _toCollect = [new TimePeriodObject(date.StartDate, _toDelete.FirstOrDefault().StartDate.AddDays(-1))];
                for (int i = 0; i < _toDelete.Count() - 1; i++)
                {
                    _toCollect.Add(new TimePeriodObject(_toDelete[i].EndDate.AddDays(1), _toDelete[i + 1].StartDate.AddDays(-1)));
                }
                _toCollect.Add(new TimePeriodObject(_toDelete.LastOrDefault().EndDate.AddDays(1), date.EndDate));

                return true;
            }
            return false;
        }

        private bool IfNotCached(TimePeriodObject date, IEnumerable<TimePeriod> timePeriods)
        {
            bool isFree = false;

            isFree = timePeriods.All(x => (x.EndDate < date.StartDate || x.StartDate > date.EndDate));
            if (isFree)
                _toCollect = [date];

            return isFree;
        }

        private bool IfCached(TimePeriodObject date, IEnumerable<TimePeriod> timePeriods)
        {
            var isExisting = timePeriods.Any(x => x.StartDate <= date.StartDate && x.EndDate >= date.EndDate);
            return isExisting;
        }

        private async Task<IEnumerable<Guid>> GetListOfFields(AddCollectedImagesCommand request)
        {
            var result = request.Fields;
            foreach (var fieldProvider in _fieldProviders)
            {
                if (result.IsNullOrEmpty())
                {
                    result = await fieldProvider.ProvideAsync();
                }
            }
            var missingFileds = request.Fields?.Where(x => result.Contains(x)).ToList();
            if (!missingFileds.IsNullOrEmpty())
            {
                foreach (var missingFiled in missingFileds)
                {
                    await _unitOfWork.FieldRepository.Add(new Field() { Id = missingFiled, Active = true });
                    await _unitOfWork.SaveAsync();
                }
            }
            return result;
        }

        private async Task<TimePeriodObject> GetTimePeriods(AddCollectedImagesCommand request)
        {
            DateTime startDate = DateTime.MinValue, endDate = DateTime.MinValue;
            DateTime? startDateOut, endDateOut;
            foreach (var periodProvider in _periodProviders)
            {
                (startDateOut, endDateOut) = await periodProvider.ProvideAsync();
                if (startDateOut is not null)
                    startDate = startDateOut.Value;
                if (endDateOut is not null)
                    endDate = endDateOut.Value;
            }

            if (request.StartDate is not null)
                startDate = request.StartDate.Value;
            if (request.EndDate is not null)
                endDate = request.EndDate.Value;
            return new TimePeriodObject(startDate, endDate);
        }
    }
}

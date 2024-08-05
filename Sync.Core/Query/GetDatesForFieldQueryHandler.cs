using AutoMapper;
using Sync.Core.Infrustructure;
using Sync.DAL;
using Sync.Services.DTOs;

namespace Sync.Core.Query
{
    public class GetDatesForFieldQueryHandler : IQueryHandler<GetDatesForFieldQuery, IEnumerable<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDatesForFieldQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<string>> Handle(GetDatesForFieldQuery request, CancellationToken cancellationToken)
        {
            var images = await _unitOfWork.ImageRepository.GetImagesByFieldId(request.FiledId);
            return _mapper.Map<List<ImageDto>>(images).OrderBy(x => x.Date).Select(x => x.Date.ToString());
        }
    }
}

using AutoMapper;
using Sync.Core.Infrustructure;
using Sync.DAL;
using Sync.Services.DTOs;

namespace Sync.Core.Query
{
    public class GetGroupedImagesQueryHandler : IQueryHandler<GetGroupedImagesQuery, IEnumerable<IGrouping<DateTime, ImageDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetGroupedImagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IGrouping<DateTime, ImageDto>>> Handle(GetGroupedImagesQuery request, CancellationToken cancellationToken)
        {
            var images = await _unitOfWork.ImageRepository.GetAll();
            return _mapper.Map<List<ImageDto>>(images)
                .OrderBy(x => x.Date)
                .GroupBy(x => x.Date);
            //.Select(g => new GroupedImagesResponce(g.Key, g.Select(x => x.Url.ToString()).ToList()));
        }
    }
}

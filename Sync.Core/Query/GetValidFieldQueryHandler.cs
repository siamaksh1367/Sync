using AutoMapper;
using Sync.Core.Infrustructure;
using Sync.DAL;

namespace Sync.Core.Query
{
    public class GetValidFieldQueryHandler : IQueryHandler<GetValidFieldQuery, IEnumerable<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetValidFieldQueryHandler(IUnitOfWork unitOfWork, IMapper mapper = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Guid>> Handle(GetValidFieldQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ImageRepository.GetImagesByDate(request.Date);
            return result.Select(x => x.Id).Distinct();

        }
    }
}

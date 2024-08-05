using Sync.DAL;

namespace Sync.Services.MultipleProviders.FiledProviders
{
    public class LocalFieldProvider : IDataProvider<IEnumerable<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocalFieldProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Guid>> ProvideAsync()
        {
            return (await _unitOfWork.FieldRepository.GetAll()).Select(x => x.Id);
        }
    }
}

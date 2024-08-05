using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Sync.Core.Infrustructure;
using Sync.DAL;
using Sync.DAL.Models;
using Sync.Services.FieldSatClient;
using Sync.Services.FieldSatClient.Caching;

namespace Sync.Core.Command
{
    public class AddCollectedFieldsCommandHandler(
        IUnitOfWork unitOfWork,
        IFieldSatClient fieldSatClient,
        ICacheManager<Guid> cacheField,
        IMapper mapper,
        ILogger<AddCollectedFieldsCommandHandler> logger) : ICommandHandler<AddCollectedFieldsCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFieldSatClient _fieldSatClient = fieldSatClient;
        private readonly ICacheManager<Guid> _cacheField = cacheField;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AddCollectedFieldsCommandHandler> _logger = logger;

        public async Task<Unit> Handle(AddCollectedFieldsCommand request, CancellationToken cancellationToken)
        {
            await RefreshCache();

            var fields = await _fieldSatClient.GetFieldsAsync();
            foreach (var field in fields)
            {
                var existingField = _cacheField.Exists(field.Id);
                if (!existingField)
                {
                    await _unitOfWork.FieldRepository.Add(_mapper.Map<Field>(field));
                    await _unitOfWork.SaveAsync();
                    _cacheField.Add(field.Id);
                }
            }
            return Unit.Value;
        }

        private async Task RefreshCache()
        {
            if (_cacheField.IsEmpty())
            {
                var existingFields = await _unitOfWork.FieldRepository.GetAll();
                foreach (var existingField in existingFields)
                {
                    _cacheField.Add(existingField.Id);
                }
            }
        }
    }
}

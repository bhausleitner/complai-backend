using System.Collections.Generic;
using System.Threading.Tasks;
using ComplAI.DataLayer.Entity;
using ComplAI.DataLayer.Interfaces;
using ComplAI.Resources.ViewModels;

namespace ComplAI.Business.Managers
{
    public class EuDocumentsManager : IManager
    {
        private readonly ICustomMongoRepository<EuDocumentEntity> _repository;
        private readonly IUnitOfWork _uow;

        public EuDocumentsManager(ICustomMongoRepository<EuDocumentEntity> repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<IEnumerable<EuDocumentEntity>> GetRegulations()
        {
            return await _repository.GetAll();
        }

    }
}

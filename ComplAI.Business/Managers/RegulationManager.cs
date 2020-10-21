using System.Collections.Generic;
using System.Threading.Tasks;
using ComplAI.DataLayer.Entity;
using ComplAI.DataLayer.Interfaces;
using ComplAI.Resources.ViewModels;

namespace ComplAI.Business.Managers
{
    public class RegulationManager : IManager
    {
        private readonly IRepository<RegulationEntity> _repository;
        private readonly IUnitOfWork _uow;

        public RegulationManager(IRepository<RegulationEntity> repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<IEnumerable<RegulationEntity>> GetRegulations()
        {
            return await _repository.GetAll();
        }

        public async Task<RegulationEntity> CreateRegulation(RegulationViewModel regulationViewModel)
        {
            var regulation = new RegulationEntity(regulationViewModel.Description, regulationViewModel.Title);
            _repository.Add(regulation);

            await _uow.Commit();

            var testProduct = await _repository.GetById(regulation.Id);

            return testProduct;
        }
    }
}

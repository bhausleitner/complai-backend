using System.Collections.Generic;
using System.Threading.Tasks;
using ComplAI.Business.Managers;
using ComplAI.Resources;
using ComplAI.Resources.Extensions;
using ComplAI.Resources.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace ComplAI.API.Controllers
{
    [Route("regulations")]
    public class RegulationController : BaseController
    {

        private readonly ILogger<RegulationController> _logger;
        private readonly RegulationManager _regulationManager;

        /// <summary>
        /// Regulations Controller
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="regulationManager"></param>
        public RegulationController(ILogger<RegulationController> logger, RegulationManager regulationManager)
        {
            _logger = logger;
            _regulationManager = regulationManager;
        }

        /// <summary>
        /// Get a list of regulations
        /// </summary>
        /// <response code="200">  A list is returned successfully </response>
        /// <response code="500">  Could not retrieve a list of regulations now  </response>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IEnumerable<RegulationResource>))]
        [Route("getAllRegulations")]
        public async Task<IEnumerable<RegulationResource>> GetAllRegulations()
        {
            var regulations =  await _regulationManager.GetRegulations();
            return regulations.ToResource();
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created)]
        public async Task<RegulationResource> CreateARegulation([FromBody] RegulationViewModel regulation)
        {
            TryValidateModel(regulation);
            var regulationEntity = await _regulationManager.CreateRegulation(regulation);
            return regulationEntity.ToResource();

        }
    }
}

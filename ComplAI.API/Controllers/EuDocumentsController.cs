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
    /// <summary>
    /// Eu documents controller 
    /// </summary>
    [Route("eu")]
    public class EuDocumentsController : BaseController
    {

        private readonly ILogger<EuDocumentsController> _logger;
        private readonly EuDocumentsManager _regulationManager;

        /// <summary>
        /// Regulations Controller
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="regulationManager"></param>
        public EuDocumentsController(ILogger<EuDocumentsController> logger, EuDocumentsManager regulationManager)
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
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IEnumerable<EuDocumentResource>))]
        [Route("getAllRegulations")]
        public async Task<IEnumerable<EuDocumentResource>> GetAllRegulations()
        {
            var regulations =  await _regulationManager.GetRegulations();
            return regulations.ToResource();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComplAI.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    public class BaseController : ControllerBase
    {
    }
}

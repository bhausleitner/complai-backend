using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ComplAI.API.Controllers
{
    [Route("authorize")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        [Route("login")]
        [HttpGet]
        public async Task Login(string returnUrl = "/")
        {

            //await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties() { RedirectUri = returnUrl, IsPersistent = true});
            await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties() { RedirectUri = returnUrl });

        }
    }
}
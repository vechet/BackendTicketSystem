using BackendTicketSystem.CustomModels;
using BackendTicketSystem.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendTicketSystem.Controllers
{
    [Authorize]
    [Route("api/v1")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;

        public AuthenticationController(IJWTAuthenticationManager jWTAuthenticationManager)
        {
            this.jWTAuthenticationManager = jWTAuthenticationManager;
        }

      
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginUserAccountCustomModel userCred)
        {
            var token = jWTAuthenticationManager.Authenticate(userCred.UserName, userCred.Password);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }
    }
}

using BackendTicketSystem.CustomModels;
using BackendTicketSystem.Data;
using BackendTicketSystem.Helpers;
using BackendTicketSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace BackendTicketSystem.Controllers
{
    [Authorize]
    [Route("api/v1")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        private readonly BackendTicketSystemContext _db = new BackendTicketSystemContext();

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


            var currentUserAccount = _db.UserAccounts.FirstOrDefault(x => x.UserName == userCred.UserName);
            //new User Account Token
            var newUserAccountToken = new UserAccountToken
            {
                Token = token,
                UserAccountId= currentUserAccount.Id
            };
            _db.UserAccountTokens.Add(newUserAccountToken);
            _db.SaveChanges();

            return Ok(token);
        }
    }
}

using System.Collections.Generic;
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
        public ApiOutput<AuthenticationCustomModel> Login([FromBody] LoginUserAccountCustomModel userCred)
        {
            try
            {
                var result = new ApiOutput<AuthenticationCustomModel>();
                var newAuthentication = new AuthenticationCustomModel();

                var token = jWTAuthenticationManager.Authenticate(userCred.UserName, userCred.Password);

                if (token == null)
                {
                    //return Unauthorized();
                    result.Success = false;
                    result.Message = Unauthorized().ToString();
                    result.Data = null;
                    return result;
                }

                var currentUserAccount = _db.UserAccounts.FirstOrDefault(x => x.UserName == userCred.UserName);
                //new User Account Token
                var newUserAccountToken = new UserAccountToken
                {
                    Token = token,
                    UserAccountId = currentUserAccount.Id
                };
                _db.UserAccountTokens.Add(newUserAccountToken);
                _db.SaveChanges();

                newAuthentication.Token = token;
                result.Success = true;
                result.Message = "Login successfully!";
                result.Data = newAuthentication;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<AuthenticationCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpGet("GetCurrentUser")]
        public ApiOutput<GetCurrentUserCustomModel> UserAccounts()
        {
            try
            {
                var result = new ApiOutput<GetCurrentUserCustomModel>();
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var currentUserId = GlobalFunction.GetCurrentUserId(_db, _bearer_token);

                var userAccounts = _db.UserAccounts.Where(x => x.Status.KeyName == "Active" && x.Id == currentUserId).Select(x => new GetCurrentUserCustomModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email= x.Email
                }).FirstOrDefault();

                result.Success = true;
                result.Message = "Fetch data successfully!";
                result.Data = userAccounts;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<GetCurrentUserCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }
    }
}

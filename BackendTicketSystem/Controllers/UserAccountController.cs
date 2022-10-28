using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendTicketSystem.CustomModels;
using BackendTicketSystem.Data;
using BackendTicketSystem.Helpers;
using BackendTicketSystem.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendTicketSystem.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly BackendTicketSystemContext _db = new BackendTicketSystemContext();

        [HttpGet("UserAccounts")]
        public ApiOutput<List<UserAccountCustomModel>> UserAccounts(int skip = 0, int limit = 10)
        {
            try
            {
                var result = new ApiOutput<List<UserAccountCustomModel>>();

                var projecTypetList = _db.UserAccounts.Where(x => x.Status.KeyName == "Active");

                var userAccounts = projecTypetList.Select(x => new UserAccountCustomModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Memo = x.Memo,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    StatusId = x.StatusId,
                    StatusName = x.Status.Name,
                    Version = x.Version
                }).OrderByDescending(x => x.Id).Skip(skip).Take(limit).ToList();

                result.Success = true;
                result.Message = "Fetch data successfully!";
                result.Data = userAccounts;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<List<UserAccountCustomModel>>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPost("UserAccountCreate")]
        public ApiOutput<CreateUserAccountCustomModel> Post([FromBody] CreateUserAccountCustomModel userAccount)
        {

            try
            {
                var result = new ApiOutput<CreateUserAccountCustomModel>();

                // check duplicate name
                if (_db.UserAccounts.Any(x => x.UserName.ToLower() == userAccount.UserName.ToLower()))
                {
                    result.Success = false;
                    result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                    result.Data = null;
                    return result;
                }

                //new project
                var newUserAccount = new UserAccount
                {
                    UserName = userAccount.UserName,
                    Memo = userAccount.Memo,
                    //CreatedBy = GlobalFunction.GetCurrentUserId(),
                    CreatedBy = 11,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                    Version = 1,
                    StatusId = userAccount.StatusId
                };
                _db.UserAccounts.Add(newUserAccount);
                _db.SaveChanges();

                result.Success = true;
                result.Message = "Created successfully!";
                result.Data = userAccount;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<CreateUserAccountCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPut("UserAccountUpdate")]
        public ApiOutput<UpdateUserAccountCustomModel> Put([FromBody] UpdateUserAccountCustomModel userAccount)
        {
            try
            {
                var result = new ApiOutput<UpdateUserAccountCustomModel>();

                var currentUserAccount = _db.UserAccounts.Find(userAccount.Id);
                if (currentUserAccount.Version == userAccount.Version)
                {
                    // check duplicate name
                    if (_db.UserAccounts.Any(x => x.UserName.ToLower() == currentUserAccount.UserName.ToLower() && x.Id != userAccount.Id))
                    {
                        result.Success = false;
                        result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                        result.Data = null;
                        return result;
                    }

                    // update project
                    currentUserAccount.UserName = userAccount.UserName;
                    currentUserAccount.Memo = userAccount.Memo;
                    currentUserAccount.StatusId = userAccount.StatusId;
                    currentUserAccount.ModifiedBy = 11;
                    //currentProject.ModifiedBy = DefaultFuntion.GetCurrentUserId();
                    currentUserAccount.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                    currentUserAccount.Version = userAccount.Version + 1;
                    _db.SaveChanges();

                    result.Success = true;
                    result.Message = "Updated successfully!";
                    result.Data = userAccount;
                    return result;
                }

                result.Success = false;
                result.Message = Resource.ValidationMessage_WrongRecordVersion;
                result.Data = null;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<UpdateUserAccountCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }
    }
}


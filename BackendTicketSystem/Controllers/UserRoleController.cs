using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendTicketSystem.CustomModels;
using BackendTicketSystem.Data;
using BackendTicketSystem.Helpers;
using BackendTicketSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendTicketSystem.Controllers
{
    [Authorize]
    [Route("api/v1")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly BackendTicketSystemContext _db = new BackendTicketSystemContext();

        [HttpGet("UserRoles")]
        public ApiOutput<List<UserRoleCustomModel>> UserRoles(int skip = 0, int limit = 10)
        {
            try
            {
                var result = new ApiOutput<List<UserRoleCustomModel>>();

                var projecTypetList = _db.UserRoles.Where(x => x.Status.KeyName == "Active");

                var userRoles = projecTypetList.Select(x => new UserRoleCustomModel
                {
                    Id = x.Id,
                    Name = x.Name,
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
                result.Data = userRoles;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<List<UserRoleCustomModel>>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPost("UserRoleCreate")]
        public ApiOutput<CreateUserRoleCustomModel> Post([FromBody] CreateUserRoleCustomModel userRole)
        {

            try
            {
                var result = new ApiOutput<CreateUserRoleCustomModel>();
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var currentUserId = GlobalFunction.GetCurrentUserId(_db, _bearer_token);

                // check duplicate name
                if (_db.UserRoles.Any(x => x.Name.ToLower() == userRole.Name.ToLower()))
                {
                    result.Success = false;
                    result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                    result.Data = null;
                    return result;
                }

                //new project
                var newUserRole = new UserRole
                {
                    Name = userRole.Name,
                    Memo = userRole.Memo,
                    CreatedBy = currentUserId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                    Version = 1,
                    StatusId = userRole.StatusId
                };
                _db.UserRoles.Add(newUserRole);
                _db.SaveChanges();

                result.Success = true;
                result.Message = "Created successfully!";
                result.Data = userRole;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<CreateUserRoleCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPut("UserRoleUpdate")]
        public ApiOutput<UpdateUserRoleCustomModel> Put([FromBody] UpdateUserRoleCustomModel userRole)
        {
            try
            {
                var result = new ApiOutput<UpdateUserRoleCustomModel>();
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var currentUserId = GlobalFunction.GetCurrentUserId(_db, _bearer_token);

                var currentUserRole = _db.UserRoles.Find(userRole.Id);
                if (currentUserRole.Version == userRole.Version)
                {
                    // check duplicate name
                    if (_db.UserRoles.Any(x => x.Name.ToLower() == currentUserRole.Name.ToLower() && x.Id != userRole.Id))
                    {
                        result.Success = false;
                        result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                        result.Data = null;
                        return result;
                    }

                    // update project
                    currentUserRole.Name = userRole.Name;
                    currentUserRole.Memo = userRole.Memo;
                    currentUserRole.StatusId = userRole.StatusId;
                    currentUserRole.ModifiedBy = currentUserId;
                    currentUserRole.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                    currentUserRole.Version = userRole.Version + 1;
                    _db.SaveChanges();

                    result.Success = true;
                    result.Message = "Updated successfully!";
                    result.Data = userRole;
                    return result;
                }

                result.Success = false;
                result.Message = Resource.ValidationMessage_WrongRecordVersion;
                result.Data = null;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<UpdateUserRoleCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }
    }
}


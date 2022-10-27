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
    public class ProjectTypeController : ControllerBase
    {
        private readonly BackendTicketSystemContext _db = new BackendTicketSystemContext();

        [HttpGet("ProjectTypes")]
        public ApiOutput<List<ProjectTypeCustomModel>> ProjectTypes(int skip = 0, int limit = 10)
        {
            try
            {
                var result = new ApiOutput<List<ProjectTypeCustomModel>>();

                var projecTypetList = _db.ProjectTypes.Where(x => x.Status.KeyName == "Active");

                var projectTypes = projecTypetList.Select(x => new ProjectTypeCustomModel
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
                result.Data = projectTypes;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<List<ProjectTypeCustomModel>>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPost("ProjectTypeCreate")]
        public ApiOutput<CreateProjectTypeCustomModel> Post([FromBody] CreateProjectTypeCustomModel projectType)
        {

            try
            {
                var result = new ApiOutput<CreateProjectTypeCustomModel>();

                // check duplicate name
                if (_db.ProjectTypes.Any(x => x.Name.ToLower() == projectType.Name.ToLower()))
                {
                    result.Success = false;
                    result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                    result.Data = null;
                    return result;
                }

                //new project
                var newProjectType = new ProjectType
                {
                    Name = projectType.Name,
                    Memo = projectType.Memo,
                    //CreatedBy = GlobalFunction.GetCurrentUserId(),
                    CreatedBy = 11,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                    Version = 1,
                    StatusId = projectType.StatusId
                };
                _db.ProjectTypes.Add(newProjectType);
                _db.SaveChanges();

                result.Success = true;
                result.Message = "Created successfully!";
                result.Data = projectType;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<CreateProjectTypeCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPut("ProjectTypeUpdate")]
        public ApiOutput<UpdateProjectTypeCustomModel> Put([FromBody] UpdateProjectTypeCustomModel projectType)
        {
            try
            {
                var result = new ApiOutput<UpdateProjectTypeCustomModel>();

                var currentProjectType = _db.ProjectTypes.Find(projectType.Id);
                if (currentProjectType.Version == projectType.Version)
                {
                    // check duplicate name
                    if (_db.ProjectTypes.Any(x => x.Name.ToLower() == currentProjectType.Name.ToLower() && x.Id != projectType.Id))
                    {
                        result.Success = false;
                        result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                        result.Data = null;
                        return result;
                    }

                    // update project
                    currentProjectType.Name = projectType.Name;
                    currentProjectType.Memo = projectType.Memo;
                    currentProjectType.StatusId = projectType.StatusId;
                    currentProjectType.ModifiedBy = 11;
                    //currentProject.ModifiedBy = DefaultFuntion.GetCurrentUserId();
                    currentProjectType.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                    currentProjectType.Version = projectType.Version + 1;
                    _db.SaveChanges();

                    result.Success = true;
                    result.Message = "Updated successfully!";
                    result.Data = projectType;
                    return result;
                }

                result.Success = false;
                result.Message = Resource.ValidationMessage_WrongRecordVersion;
                result.Data = null;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<UpdateProjectTypeCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }
    }
}


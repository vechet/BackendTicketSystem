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
    public class ProjectPackageController : ControllerBase
    {
        private BackendTicketSystemContext _db = new BackendTicketSystemContext();

        [HttpGet("ProjectPackages")]
        public ApiOutput<List<ProjectPackageCustomModel>> ProjectPackages(int skip = 0, int limit = 10)
        {
            try
            {
                var result = new ApiOutput<List<ProjectPackageCustomModel>>();

                var projecPackagetList = _db.ProjectPackages.Where(x => x.Status.KeyName == "Active");

                var projectPackages = projecPackagetList.Select(x => new ProjectPackageCustomModel
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
                result.Data = projectPackages;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<List<ProjectPackageCustomModel>>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPost("ProjectPackageCreate")]
        public ApiOutput<CreateProjectPackageCustomModel> Post([FromBody] CreateProjectPackageCustomModel projectPackage)
        {

            try
            {
                var result = new ApiOutput<CreateProjectPackageCustomModel>();

                // check duplicate name
                if (_db.ProjectPackages.Any(x => x.Name.ToLower() == projectPackage.Name.ToLower()))
                {
                    result.Success = false;
                    result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                    result.Data = null;
                    return result;
                }

                //new project
                var newProjectPackage = new ProjectPackage
                {
                    Name = projectPackage.Name,
                    Memo = projectPackage.Memo,
                    //CreatedBy = GlobalFunction.GetCurrentUserId(),
                    CreatedBy = 11,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                    Version = 1,
                    StatusId = projectPackage.StatusId
                };
                _db.ProjectPackages.Add(newProjectPackage);
                _db.SaveChanges();

                result.Success = true;
                result.Message = "Created successfully!";
                result.Data = projectPackage;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<CreateProjectPackageCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPut("ProjectPackageUpdate")]
        public ApiOutput<UpdateProjectPackageCustomModel> Put([FromBody] UpdateProjectPackageCustomModel projectPackage)
        {
            try
            {
                var result = new ApiOutput<UpdateProjectPackageCustomModel>();

                var currentProjectPackage = _db.ProjectPackages.Find(projectPackage.Id);
                if (currentProjectPackage.Version == projectPackage.Version)
                {
                    // check duplicate name
                    if (_db.ProjectPackages.Any(x => x.Name.ToLower() == currentProjectPackage.Name.ToLower() && x.Id != projectPackage.Id))
                    {
                        result.Success = false;
                        result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                        result.Data = null;
                        return result;
                    }

                    // update project
                    currentProjectPackage.Name = projectPackage.Name;
                    currentProjectPackage.Memo = projectPackage.Memo;
                    currentProjectPackage.StatusId = projectPackage.StatusId;
                    currentProjectPackage.ModifiedBy = 11;
                    //currentProject.ModifiedBy = DefaultFuntion.GetCurrentUserId();
                    currentProjectPackage.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                    currentProjectPackage.Version = projectPackage.Version + 1;
                    _db.SaveChanges();

                    result.Success = true;
                    result.Message = "Updated successfully!";
                    result.Data = projectPackage;
                    return result;
                }

                result.Success = false;
                result.Message = Resource.ValidationMessage_WrongRecordVersion;
                result.Data = null;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<UpdateProjectPackageCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }
    }
}


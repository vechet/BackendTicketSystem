using BackendTicketSystem.CustomModels;
using BackendTicketSystem.Data;
using BackendTicketSystem.Helpers;
using BackendTicketSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Enum = BackendTicketSystem.Helpers.Enum;

namespace BackendTicketSystem.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly BackendTicketSystemContext _db = new BackendTicketSystemContext();

        [HttpGet("Projects")]
        public ApiOutput<List<ProjectCustomModel>> Projects(int skip = 0, int limit = 10)
        {
            try
            {
                var result = new ApiOutput<List<ProjectCustomModel>>();

                var projectList = _db.Projects.Where(x => x.Status.KeyName == "Active");

                var projects = projectList.Select(x => new ProjectCustomModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProjectTypeId = x.ProjectTypeId,
                    ProjectTypeName = x.ProjectType.Name,
                    ProjectPackageId = x.ProjectPackageId,
                    ProjectPackageName = x.ProjectPackage.Name,
                    WebsiteUrl = x.WebsiteUrl,
                    ApplicationName = x.ApplicationName,
                    DatabaseName = x.DatabaseName,
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
                result.Data = projects;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<List<ProjectCustomModel>>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPost("ProjectCreate")]
        public ApiOutput<CreateProjectCustomModel> Post([FromBody] CreateProjectCustomModel project)
        {

            try
            {
                var result = new ApiOutput<CreateProjectCustomModel>();

                // check duplicate name
                if (_db.Projects.Any(x => x.Name.ToLower() == project.Name.ToLower()))
                {
                    result.Success = false;
                    result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                    result.Data = null;
                    return result;
                }

                //new project
                var newProject = new Project
                {
                    Name = project.Name,
                    ProjectTypeId = project.ProjectTypeId,
                    ProjectPackageId = project.ProjectPackageId,
                    WebsiteUrl = project.WebsiteUrl,
                    ApplicationName = project.ApplicationName,
                    DatabaseName = project.DatabaseName,
                    //CreatedBy = GlobalFunction.GetCurrentUserId(),
                    CreatedBy = 11,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                    Version = 1,
                    StatusId = project.StatusId
                };
                _db.Projects.Add(newProject);
                _db.SaveChanges();

                result.Success = true;
                result.Message = "Created successfully!";
                result.Data = project;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<CreateProjectCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPut("ProjectUpdate")]
        public ApiOutput<UpdateProjectCustomModel> Put([FromBody] UpdateProjectCustomModel project)
        {
            try
            {
                var result = new ApiOutput<UpdateProjectCustomModel>();

                var currentProject = _db.Projects.Find(project.Id);
                if (currentProject.Version == project.Version)
                {
                    // check duplicate name
                    if (_db.Projects.Any(x => x.Name.ToLower() == currentProject.Name.ToLower() && x.Id != project.Id))
                    {
                        result.Success = false;
                        result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                        result.Data = null;
                        return result;
                    }

                    // update project
                    currentProject.Name = project.Name;
                    currentProject.ProjectTypeId = project.ProjectTypeId;
                    currentProject.ProjectPackageId = project.ProjectPackageId;
                    currentProject.WebsiteUrl = project.WebsiteUrl;
                    currentProject.ApplicationName = project.ApplicationName;
                    currentProject.DatabaseName = project.DatabaseName;
                    currentProject.StatusId = project.StatusId;
                    currentProject.ModifiedBy = 11;
                    //currentProject.ModifiedBy = DefaultFuntion.GetCurrentUserId();
                    currentProject.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                    currentProject.Version = project.Version + 1;
                    _db.SaveChanges();

                    result.Success = true;
                    result.Message = "Updated successfully!";
                    result.Data = project;
                    return result;
                }

                result.Success = false;
                result.Message = Resource.ValidationMessage_WrongRecordVersion;
                result.Data = null;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<UpdateProjectCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

    }
}

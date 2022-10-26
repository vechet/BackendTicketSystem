using BackendTicketSystem.CustomModels;
using BackendTicketSystem.Data;
using BackendTicketSystem.Helpers;
using BackendTicketSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Enum = BackendTicketSystem.Helpers.Enum;

namespace BackendTicketSystem.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private BackendTicketSystemContext _db = new BackendTicketSystemContext();

        [HttpGet("Projects")]
        public IEnumerable<ProjectCustomModel>? Projects(int skip = 0, int limit = 10)
        {
            var projectList = _db.Projects.Where(x => x.Status.KeyName == "Active");

            var result = projectList.Select(x => new ProjectCustomModel
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
            }).OrderByDescending(x => x.Id).Skip(skip).Take(limit);

            return result;
        }

        [HttpPost("projectCreate")]
        public string Post([FromBody] CreateProjectCustomModel project)
        {
            try
            {
                // check duplicate name
                if (_db.Projects.Any(x => x.Name.ToLower() == project.Name.ToLower()))
                {
                    return string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
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
                return "create successfully";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [HttpPut("projectUpdate")]
        public string Put([FromBody] UpdateProjectCustomModel project)
        {
            try
            {
                var currentProject = _db.Projects.Find(project.Id);
                if (currentProject.Version == project.Version)
                {
                    // check duplicate name
                    if (_db.Projects.Any(x => x.Name.ToLower() == currentProject.Name.ToLower() && x.Id != project.Id))
                    {
                        return string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
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
                    return "update successfully";
                }
                return Resource.ValidationMessage_WrongRecordVersion;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

    }
}

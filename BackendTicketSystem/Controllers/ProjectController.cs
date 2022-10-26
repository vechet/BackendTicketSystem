using BackendTicketSystem.CustomModels;
using BackendTicketSystem.Data;
using BackendTicketSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendTicketSystem.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly BackendTicketSystemContext _db = new BackendTicketSystemContext();

        [HttpGet("Projects")]
        public IEnumerable<ProjectCustomModel>? Projects(int skip = 0, int limit = 10)
        {
            try
            {
                var projectList = _db.Projects.Where(x => x.Status.KeyName == "Active");

                var result = projectList.Select(x => new ProjectCustomModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProjectTypeId=x.ProjectTypeId,
                    ProjectTypeName=x.ProjectType.Name,
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
                    Version=x.Version
                }).OrderByDescending(x => x.Id).Skip(skip).Take(limit);

                return result;
            }
            catch (Exception ex)
            {
                // ignored
            }
            return null;
        }

        

    }
}

using System;
namespace BackendTicketSystem.CustomModels
{
    public class ProjectCustomModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; } = null!;
        public int ProjectPackageId { get; set; }
        public string ProjectPackageName { get; set; } = null!;
        public string WebsiteUrl { get; set; } = null!;
        public string ApplicationName { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public short StatusId { get; set; }
        public string StatusName { get; set; } = null!;
        public int Version { get; set; }
    }
}


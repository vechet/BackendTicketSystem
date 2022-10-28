using System;
namespace BackendTicketSystem.CustomModels
{
    public class ProjectTypeCustomModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Memo { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public short StatusId { get; set; }
        public string StatusName { get; set; } = null!;
        public int Version { get; set; }
    }

    public class CreateProjectTypeCustomModel
    {
        public string Name { get; set; } = null!;
        public string? Memo { get; set; }
        public short StatusId { get; set; }
    }

    public class UpdateProjectTypeCustomModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Memo { get; set; }
        public short StatusId { get; set; }
        public int Version { get; set; }
    }
}


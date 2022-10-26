using System;
using System.Collections.Generic;

namespace BackendTicketSystem.Models
{
    public partial class ProjectPackage
    {
        public ProjectPackage()
        {
            Projects = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Memo { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public short StatusId { get; set; }
        public int Version { get; set; }

        public virtual UserAccount CreatedByNavigation { get; set; } = null!;
        public virtual UserAccount? ModifiedByNavigation { get; set; }
        public virtual Status Status { get; set; } = null!;
        public virtual ICollection<Project> Projects { get; set; }
    }
}

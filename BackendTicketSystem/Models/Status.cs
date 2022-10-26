using System;
using System.Collections.Generic;

namespace BackendTicketSystem.Models
{
    public partial class Status
    {
        public Status()
        {
            GlobalParams = new HashSet<GlobalParam>();
            ProjectPackages = new HashSet<ProjectPackage>();
            ProjectTypes = new HashSet<ProjectType>();
            Projects = new HashSet<Project>();
            TicketTypes = new HashSet<TicketType>();
            Tickets = new HashSet<Ticket>();
            UserAccounts = new HashSet<UserAccount>();
            UserRoles = new HashSet<UserRole>();
        }

        public short Id { get; set; }
        public string Name { get; set; } = null!;
        public string KeyName { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Version { get; set; }

        public virtual ICollection<GlobalParam> GlobalParams { get; set; }
        public virtual ICollection<ProjectPackage> ProjectPackages { get; set; }
        public virtual ICollection<ProjectType> ProjectTypes { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<TicketType> TicketTypes { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<UserAccount> UserAccounts { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}

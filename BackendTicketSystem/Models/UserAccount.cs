using System;
using System.Collections.Generic;

namespace BackendTicketSystem.Models
{
    public partial class UserAccount
    {
        public UserAccount()
        {
            ProjectCreatedByNavigations = new HashSet<Project>();
            ProjectModifiedByNavigations = new HashSet<Project>();
            ProjectPackageCreatedByNavigations = new HashSet<ProjectPackage>();
            ProjectPackageModifiedByNavigations = new HashSet<ProjectPackage>();
            ProjectTypeCreatedByNavigations = new HashSet<ProjectType>();
            ProjectTypeModifiedByNavigations = new HashSet<ProjectType>();
            TicketActions = new HashSet<TicketAction>();
            TicketCreatedByNavigations = new HashSet<Ticket>();
            TicketModifiedByNavigations = new HashSet<Ticket>();
            TicketOpennedByNavigations = new HashSet<Ticket>();
            TicketTypeCreatedByNavigations = new HashSet<TicketType>();
            TicketTypeModifiedByNavigations = new HashSet<TicketType>();
            UserAccountTokens = new HashSet<UserAccountToken>();
            UserRoleCreatedByNavigations = new HashSet<UserRole>();
            UserRoleModifiedByNavigations = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public int GenderId { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public string? Address { get; set; }
        public int UserRoleId { get; set; }
        public string? Memo { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public short StatusId { get; set; }
        public int Version { get; set; }

        public virtual GlobalParam Gender { get; set; } = null!;
        public virtual Status Status { get; set; } = null!;
        public virtual UserRole UserRole { get; set; } = null!;
        public virtual ICollection<Project> ProjectCreatedByNavigations { get; set; }
        public virtual ICollection<Project> ProjectModifiedByNavigations { get; set; }
        public virtual ICollection<ProjectPackage> ProjectPackageCreatedByNavigations { get; set; }
        public virtual ICollection<ProjectPackage> ProjectPackageModifiedByNavigations { get; set; }
        public virtual ICollection<ProjectType> ProjectTypeCreatedByNavigations { get; set; }
        public virtual ICollection<ProjectType> ProjectTypeModifiedByNavigations { get; set; }
        public virtual ICollection<TicketAction> TicketActions { get; set; }
        public virtual ICollection<Ticket> TicketCreatedByNavigations { get; set; }
        public virtual ICollection<Ticket> TicketModifiedByNavigations { get; set; }
        public virtual ICollection<Ticket> TicketOpennedByNavigations { get; set; }
        public virtual ICollection<TicketType> TicketTypeCreatedByNavigations { get; set; }
        public virtual ICollection<TicketType> TicketTypeModifiedByNavigations { get; set; }
        public virtual ICollection<UserAccountToken> UserAccountTokens { get; set; }
        public virtual ICollection<UserRole> UserRoleCreatedByNavigations { get; set; }
        public virtual ICollection<UserRole> UserRoleModifiedByNavigations { get; set; }
    }
}

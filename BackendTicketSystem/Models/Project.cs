﻿using System;
using System.Collections.Generic;

namespace BackendTicketSystem.Models
{
    public partial class Project
    {
        public Project()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ProjectTypeId { get; set; }
        public int ProjectPackageId { get; set; }
        public string WebsiteUrl { get; set; } = null!;
        public string ApplicationName { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public short StatusId { get; set; }
        public int Version { get; set; }

        public virtual UserAccount CreatedByNavigation { get; set; } = null!;
        public virtual UserAccount? ModifiedByNavigation { get; set; }
        public virtual ProjectPackage ProjectPackage { get; set; } = null!;
        public virtual ProjectType ProjectType { get; set; } = null!;
        public virtual Status Status { get; set; } = null!;
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
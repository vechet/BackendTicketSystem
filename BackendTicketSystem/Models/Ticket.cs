using System;
using System.Collections.Generic;

namespace BackendTicketSystem.Models
{
    public partial class Ticket
    {
        public Ticket()
        {
            TicketActions = new HashSet<TicketAction>();
        }

        public int Id { get; set; }
        public string Subject { get; set; } = null!;
        public int PriorityId { get; set; }
        public int ProjectId { get; set; }
        public int TicketTypeId { get; set; }
        public bool Severity { get; set; }
        public int TransactionType { get; set; }
        public int OpennedBy { get; set; }
        public DateTime OpennedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public short StatusId { get; set; }
        public int Version { get; set; }

        public virtual UserAccount CreatedByNavigation { get; set; } = null!;
        public virtual UserAccount? ModifiedByNavigation { get; set; }
        public virtual UserAccount OpennedByNavigation { get; set; } = null!;
        public virtual GlobalParam Priority { get; set; } = null!;
        public virtual Project Project { get; set; } = null!;
        public virtual Status Status { get; set; } = null!;
        public virtual TicketType TicketType { get; set; } = null!;
        public virtual GlobalParam TransactionTypeNavigation { get; set; } = null!;
        public virtual ICollection<TicketAction> TicketActions { get; set; }
    }
}

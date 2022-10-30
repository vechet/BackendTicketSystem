using System;
using System.Collections.Generic;

namespace BackendTicketSystem.Models
{
    public partial class GlobalParam
    {
        public GlobalParam()
        {
            TicketPriorities = new HashSet<Ticket>();
            TicketTransactionTypeNavigations = new HashSet<Ticket>();
            UserAccounts = new HashSet<UserAccount>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string KeyName { get; set; } = null!;
        public string? Type { get; set; }
        public string? Memo { get; set; }
        public short StatusId { get; set; }

        public virtual Status Status { get; set; } = null!;
        public virtual ICollection<Ticket> TicketPriorities { get; set; }
        public virtual ICollection<Ticket> TicketTransactionTypeNavigations { get; set; }
        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}

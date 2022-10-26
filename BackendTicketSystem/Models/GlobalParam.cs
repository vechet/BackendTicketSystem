using System;
using System.Collections.Generic;

namespace BackendTicketSystem.Models
{
    public partial class GlobalParam
    {
        public GlobalParam()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string KeyName { get; set; } = null!;
        public string? Type { get; set; }
        public string? Memo { get; set; }
        public short StatusId { get; set; }

        public virtual Status Status { get; set; } = null!;
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}

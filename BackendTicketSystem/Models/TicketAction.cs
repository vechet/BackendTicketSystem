using System;
using System.Collections.Generic;

namespace BackendTicketSystem.Models
{
    public partial class TicketAction
    {
        public TicketAction()
        {
            TicketActionFiles = new HashSet<TicketActionFile>();
        }

        public int Id { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; } = null!;
        public DateTime TransactionDate { get; set; }
        public int TransactionType { get; set; }

        public virtual Ticket Ticket { get; set; } = null!;
        public virtual ICollection<TicketActionFile> TicketActionFiles { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace BackendTicketSystem.Models
{
    public partial class TicketActionFile
    {
        public int Id { get; set; }
        public int? TicketActionId { get; set; }
        public string FileName { get; set; } = null!;
        public string FileExtension { get; set; } = null!;

        public virtual TicketAction? TicketAction { get; set; }
    }
}

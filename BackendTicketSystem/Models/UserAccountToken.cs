using System;
using System.Collections.Generic;

namespace BackendTicketSystem.Models
{
    public partial class UserAccountToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public int UserAccountId { get; set; }

        public virtual UserAccount UserAccount { get; set; } = null!;
    }
}

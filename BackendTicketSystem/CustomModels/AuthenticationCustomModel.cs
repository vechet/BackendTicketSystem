using System;
namespace BackendTicketSystem.CustomModels
{
    public class AuthenticationCustomModel
    {
        public string Token { get; set; } = null!;
    }

    public class GetCurrentUserCustomModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

}


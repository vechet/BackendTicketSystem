using System;
namespace BackendTicketSystem.CustomModels
{
    public class TicketCustomModel
    {
        public int Id { get; set; }
        public string Summary { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int PriorityId { get; set; }
        public string PriorityName { get; set; } = null!;
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public int TicketTypeId { get; set; }
        public string TicketTypeName { get; set; } = null!;
        public bool Severity { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public short StatusId { get; set; }
        public string StatusName { get; set; } = null!;
        public int Version { get; set; }
    }

    public class CreateTicketCustomModel
    {
        public string Summary { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int PriorityId { get; set; }
        public int ProjectId { get; set; }
        public int TicketTypeId { get; set; }
        public bool Severity { get; set; }
        public short StatusId { get; set; }
    }

    public class UpdateTicketCustomModel
    {
        public int Id { get; set; }
        public string Summary { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int PriorityId { get; set; }
        public int ProjectId { get; set; }
        public int TicketTypeId { get; set; }
        public bool Severity { get; set; }
        public short StatusId { get; set; }
        public int Version { get; set; }
    }

    public class TicketPriorityCustomModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}


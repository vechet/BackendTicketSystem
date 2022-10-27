namespace BackendTicketSystem.Helpers
{
     public class ApiOutput<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        
    }
}

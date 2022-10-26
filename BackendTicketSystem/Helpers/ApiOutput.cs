namespace BackendTicketSystem.Helpers
{
     public class ApiOutput<T>
    {
        public T Result { get; set; }
        public string TargetUrl { get; set; }
        public bool Success { get; set; }
        public ApiErrorResultOutput Error { get; set; }
        public bool UnAuthorizedRequest { get; set; }
        public bool _abp { get; set; }
    }

    public class ApiErrorResultOutput
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public List<ApiValidationErrorResultOutput> ValidationErrors { get; set; }
    }

    public class ApiValidationErrorResultOutput
    {
        public string Message { get; set; }
        public string[] Members { get; set; }
    }
}

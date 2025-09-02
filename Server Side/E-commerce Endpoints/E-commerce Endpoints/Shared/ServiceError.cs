namespace E_commerce_Endpoints.Shared
{
    public class ServiceError
    {
        public ServiceErrorType Type { get; set; } = ServiceErrorType.None;
        public string Message { get; set; } = string.Empty;
    }
}

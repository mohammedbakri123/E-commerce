namespace E_commerce_Endpoints.Shared
{
    public class ServiceResult<T>
    {
        public bool Success => Error.Type == ServiceErrorType.None;
        public ServiceError Error { get; set; } = new ServiceError();
        public T Data { get; set; }

        public static ServiceResult<T> Ok(T data) => new ServiceResult<T> { Data = data, Error = new ServiceError() };
        public static ServiceResult<T> Fail(ServiceErrorType type, string message) =>
            new ServiceResult<T> { Error = new ServiceError { Type = type, Message = message } };
    }
}

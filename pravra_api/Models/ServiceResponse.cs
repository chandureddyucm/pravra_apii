namespace pravra_api.Models
{
    public class ServiceResponse<T>
    {
        public bool Status { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;

        public ServiceResponse<T> SetResponse(bool status, string message, T? data = default)
        {
            Status = status;
            Data = data;
            Message = message;
            return this;
        }
        public ServiceResponse<T> SetResponse(bool status, T? data = default)
        {
            Status = status;
            Data = data;
            return this;
        }
        public ServiceResponse<T> SetResponseWithEx(string error)
        {
            Status = false;
            Error = error;
            return this;
        }
    }

    public class ServiceResponseLogin<T> : ServiceResponse<T>
    {
        public string BearerToken { get; set; } = string.Empty;
        public ServiceResponseLogin<T> SetResponse(bool status, string message, string? bearerToken, T? data = default)
        {
            base.SetResponse(status, message, data);
            BearerToken = bearerToken ?? string.Empty;
            return this;
        }
        public ServiceResponseLogin<T> SetResponse(bool status, string message)
        {
            base.SetResponse(status, message);
            return this;
        }
        public ServiceResponseLogin<T> SetResponseWithEx(string error, string bearerToken = "")
        {
            base.SetResponseWithEx(error);
            BearerToken = bearerToken;
            return this;
        }
    }
}
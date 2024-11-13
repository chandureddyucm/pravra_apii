namespace pravra_api.Models
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; }
    }

    public class ServiceResponseLogin<T> : ServiceResponse<T> {
        public string? BearerToken { get; set; }
    }
}
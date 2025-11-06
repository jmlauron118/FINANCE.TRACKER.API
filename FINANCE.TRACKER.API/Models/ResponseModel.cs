namespace FINANCE.TRACKER.API.Models
{
    public class ResponseModel<T>
    {
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}

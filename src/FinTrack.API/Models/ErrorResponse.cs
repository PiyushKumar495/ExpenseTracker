namespace FinTrack.API.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public required string Code { get; set; }
        public required string Message { get; set; }
        public List<string>? Errors { get; set; }
    }
}
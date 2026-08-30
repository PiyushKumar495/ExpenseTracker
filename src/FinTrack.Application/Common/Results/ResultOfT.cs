// for operation which returns data but can also fail like authenticaiton
namespace FinTrack.Application.Common.Results
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T? Value { get; set; }
        public Error? Error { get; set; }

    }
}
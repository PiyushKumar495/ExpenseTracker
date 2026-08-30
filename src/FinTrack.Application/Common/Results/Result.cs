//for logout function which dont return any data object but can successs or fail
namespace FinTrack.Application.Common.Results
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public Error? Error { get; set; }

    }
}
namespace FinTrack.Application.DTOs.Common
{
    public class SortingRequest
    {
        public string SortBy { get; set; } = "TransactionDate";
        public string SortOrder { get; set; } = "desc";
    }
}
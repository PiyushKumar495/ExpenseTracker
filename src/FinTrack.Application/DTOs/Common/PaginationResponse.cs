namespace FinTrack.Application.DTOs.Common
{
    public class PaginationResponse
    {
        public int Pagenumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
namespace EasyWater.Domain.Models.Api
{
    public abstract class PaginationReport
    {
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}

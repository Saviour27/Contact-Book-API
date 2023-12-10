namespace ContactBookApi.Domain.DTOs
{
    public class PaginationFilter
    {
        public int CurrentPage { get; set; }
        public PaginationFilter()
        {
            this.CurrentPage = 1;
        }
        public PaginationFilter(int CurrentPage)
        {
            this.CurrentPage = CurrentPage <= 0 ? 1 : CurrentPage;
        }
    }
}

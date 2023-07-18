namespace Business_AnimeToNotion.Model.Pagination
{
    public class PaginatedResponse<T>
    {
        public PageInfo PageInfo { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PaginatedResponse()
        {
            
        }
    }

    public class PageInfo
    {        
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int PerPage { get; set; }
        public int LastPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}

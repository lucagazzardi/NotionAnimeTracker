namespace Business_AnimeToNotion.Model.Query
{
    public class PageIn
    {
        public int CurrentPage { get; set; }
        public int PerPage { get; set; } = 20;
        public int? TotalCount { get; set; }

    }
}

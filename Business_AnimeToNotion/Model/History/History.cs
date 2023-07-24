namespace Business_AnimeToNotion.Model.History
{
    public class HistoryYear
    {
        public int Year { get; set; }
        public int WatchedNumber { get; set; }
        public int FavoritesNumber { get; set; }
        public List<HistoryYearPreview> Data { get; set; }
    }

    public class HistoryYearPreview
    {
        public int MalId { get; set; }
        public string NameEnglish { get; set; }
        public string Cover { get; set; }
    }
}

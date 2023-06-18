namespace Data_AnimeToNotion.DataModel
{
    public class Year
    {
        public Guid Id { get; set; }
        public string NotionPageId { get; set; }
        public int YearValue { get; set; }

        public ICollection<WatchingTime> WatchingTimes { get; set; }
    }
}

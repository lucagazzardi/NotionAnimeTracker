namespace Business_AnimeToNotion.Model.Internal
{
    public class INT_AnimeEpisode
    {
        public Guid? Id { get; set; }
        public Guid AnimeShowId { get; set; }
        public int EpisodeNumber { get; set; }
        public DateTime WatchedOn { get; set; }
    }
}

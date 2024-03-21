namespace Business_AnimeToNotion.Model.Internal
{
    public class INT_AnimeEpisode
    {
        public int? Id { get; set; }
        public int AnimeShowId { get; set; }
        public int EpisodeNumber { get; set; }
        public DateTime WatchedOn { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace Data_AnimeToNotion.DataModel
{
    [Index(nameof(AnimeShowId), nameof(EpisodeNumber), IsUnique = true)]
    public class AnimeEpisode
    {
        public Guid Id { get; set; }
        public Guid AnimeShowId { get; set; }
        public int EpisodeNumber { get; set; }
        public DateTime WatchedOn { get; set; }

        public AnimeShow AnimeShow { get; set; }
    }
}

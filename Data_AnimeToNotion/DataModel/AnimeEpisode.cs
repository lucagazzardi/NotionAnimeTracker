using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    public class AnimeEpisode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AnimeShowId { get; set; }
        public int EpisodeNumber { get; set; }
        public DateTime WatchedOn { get; set; }

        public AnimeShow AnimeShow { get; set; }
    }
}

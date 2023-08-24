using Microsoft.EntityFrameworkCore;

namespace Data_AnimeToNotion.DataModel
{
    [Index(nameof(AnimeShowId), nameof(StudioId), IsUnique = true)]
    public class StudioOnAnimeShow
    {
        public Guid Id { get; set; }
        public Guid AnimeShowId { get; set; }
        public int StudioId { get; set; }
        public string Description { get; set; }

        public AnimeShow AnimeShow { get; set; }
        public Studio Studio { get; set; }
    }
}

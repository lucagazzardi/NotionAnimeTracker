using Microsoft.EntityFrameworkCore;

namespace Data_AnimeToNotion.DataModel
{
    [Index(nameof(AnimeShowId), nameof(GenreId), IsUnique = true)]
    public class GenreOnAnimeShow
    {        
        public Guid Id { get; set; }
        public Guid AnimeShowId { get; set; }
        public int GenreId { get; set; }
        public string Description { get; set; }

        public AnimeShow AnimeShow { get; set; }
        public Genre Genre { get; set; }
    }
}

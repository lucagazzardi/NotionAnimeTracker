using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    public class GenreOnAnimeShow
    {
        [Column(Order = 0)]
        public Guid Id { get; set; }

        [Column(Order = 1)]
        public Guid AnimeShowId { get; set; }

        [Column(Order = 2)]
        public Guid GenreId { get; set; }


        public AnimeShow AnimeShow { get; set; }
        public Genre Genre { get; set; }
    }
}

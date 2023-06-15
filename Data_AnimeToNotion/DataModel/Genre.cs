using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    public class Genre
    {
        [Column(Order = 0)]
        public Guid Id { get; set; }

        [Column(Order = 1)]
        public int MalId { get; set; }

        [Column(Order = 2)]
        public string Description { get; set; }

        public ICollection<GenreOnAnimeShow> GenreOnAnimeShows { get; set; }

    }
}

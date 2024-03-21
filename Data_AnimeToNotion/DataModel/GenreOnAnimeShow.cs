using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{    
    public class GenreOnAnimeShow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AnimeShowId { get; set; }
        public int GenreId { get; set; }
        public string Description { get; set; }

        public AnimeShow AnimeShow { get; set; }
        public Genre Genre { get; set; }
    }
}

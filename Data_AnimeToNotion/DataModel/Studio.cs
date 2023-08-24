using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    public class Studio
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }     
        public string Description { get; set; }

        public ICollection<StudioOnAnimeShow> StudioOnAnimeShows { get; set; }

    }
}

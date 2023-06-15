using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    public class StudioOnAnimeShow
    {
        [Column(Order = 0)]
        public Guid Id { get; set; }

        [Column(Order = 1)]
        public Guid AnimeShowId { get; set; }

        [Column(Order = 2)]
        public Guid StudioId { get; set; }


        public AnimeShow AnimeShow { get; set; }
        public Studio Studio { get; set; }
    }
}

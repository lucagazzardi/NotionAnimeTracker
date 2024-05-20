using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    public class AnimeShowProgress
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AnimeShowId { get; set; }
        public int? EpisodesProgress { get; set; }
        public int? PersonalScore { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? FinishedOn { get; set; }
        public int? CompletedYear { get; set; }
        public string Notes { get; set; }

        public AnimeShow AnimeShow { get; set; }
    }
}

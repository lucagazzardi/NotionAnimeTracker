using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    public class WatchingTime
    {
        [Column(Order = 0)]
        public Guid Id { get; set; }

        [Column(Order = 1)]
        public DateTime StartedOn { get; set; }

        [Column(Order = 2)]
        public DateTime? FinishedOn { get; set; }

        [Column(Order = 3)]
        [ForeignKey("Year")]
        public Guid? CompletedYear { get; set; }


        public AnimeShow AnimeShow { get; set; }
        public Year Year { get; set; }

    }
}

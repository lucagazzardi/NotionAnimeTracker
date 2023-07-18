using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    public class WatchingTime
    {
        public Guid Id { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime? FinishedOn { get; set; }
        public int? CompletedYear { get; set; }

        public AnimeShow AnimeShow { get; set; }

    }
}

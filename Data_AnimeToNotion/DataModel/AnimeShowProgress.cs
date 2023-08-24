namespace Data_AnimeToNotion.DataModel
{
    public class AnimeShowProgress
    {
        public Guid Id { get; set; }
        public Guid AnimeShowId { get; set; }
        public int? PersonalScore { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? FinishedOn { get; set; }
        public int? CompletedYear { get; set; }
        public string Notes { get; set; }

        public AnimeShow AnimeShow { get; set; }
    }
}

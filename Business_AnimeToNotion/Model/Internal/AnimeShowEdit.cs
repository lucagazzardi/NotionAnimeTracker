namespace Business_AnimeToNotion.Model.Internal
{
    public class AnimeShowEdit
    {
        public int? Id { get; set; }
        public string? Status { get; set; }
        public int? EpisodesProgress { get; set; }
        public int? PersonalScore { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? FinishedOn { get; set; }
        public string? Notes { get; set; }
        public int? CompletedYear { get; set; }
    }
}

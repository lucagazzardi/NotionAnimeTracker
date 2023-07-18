namespace Business_AnimeToNotion.Model.Query.Filter
{
    public class FilterIn
    {
        public string? Search { get; set; }
        public string? Status { get; set; }
        public string? Format { get; set; }
        public int? Year { get; set; }
        public int? MalScoreGreater { get; set; }
        public int? MalScoreLesser { get; set; }
        public int? PersonalScoreGreater { get; set; }
        public int? PersonalScoreLesser { get; set; }
        public int? EpisodesGreater { get; set; }
        public int? EpisodesLesser { get; set; }
        public bool? FavoritesOnly { get; set; }
        public bool? PlanToWatchOnly { get; set; }

    }
}

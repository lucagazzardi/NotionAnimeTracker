namespace Data_AnimeToNotion.Model
{
    public class AnimeShowDto
    {   
        public string NotionPageId { get; set; }        
        public int MalId { get; set; }        
        public string NameOriginal { get; set; }        
        public string NameEnglish { get; set; }        
        public string Format { get; set; }        
        public int? Episodes { get; set; }
        public string Status { get; set; }
        public DateTime? StartedAiring { get; set; }
        public string Cover { get; set; }
        public ScoreDto Score { get; set; }
        public WatchingTimeDto WatchingTime { get; set; }
        public NoteDto Note { get; set; }
        public Dictionary<int, string> Genres { get; set; }
        public Dictionary<int, string> Studios { get; set; }
        public List<RelationDto> Relations { get; set; }
    }

    public class ScoreDto
    {
        public int MalScore { get; set; }
        public int? PersonalScore { get; set; }
        public bool? Favorite { get; set; }
    }

    public class WatchingTimeDto
    {
        public DateTime StartedOn { get; set; }
        public DateTime? FinishedOn { get; set; }
        public int? CompletedYear { get; set; }
    }

    public class NoteDto
    {
        public string Notes { get; set; }
    }

    public class RelationDto
    {
        public Guid AnimeShowId { get; set; }
        public int AnimeRelatedMalId { get; set; }
        public string RelationType { get; set; }
    }

    public enum Format
    {
        tv = 0,
        ova = 1,
        movie = 2,
        special = 3
    }

    public enum Status
    {
        towatch = 0,
        watching = 1,
        completed = 2,
        dropped = 3
    }
}

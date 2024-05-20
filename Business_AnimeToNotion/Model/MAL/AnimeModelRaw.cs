namespace Business_AnimeToNotion.Model.MAL
{
    public class AnimeShowRaw
    {
        public int id { get; set; }
        public string title { get; set; }
        public MainPicture main_picture { get; set; }
        public AlternativeTitle alternative_titles { get; set; }
        public decimal mean { get; set; }
        public string start_date { get; set; }
        public string media_type { get; set; }
        public List<GeneralObject> genres { get; set; } = new List<GeneralObject>();
        public List<GeneralObject> studios { get; set; } = new List<GeneralObject>();
        public int num_episodes { get; set; }
    }

    public class AlternativeTitle
    {
        public string[] synonyms { get; set; }
        public string en { get; set; }
        public string ja { get; set; }
    }

    public class MainPicture
    {
        public string medium { get; set; }
        public string large { get; set; }
    }    

    public class GeneralObject
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}

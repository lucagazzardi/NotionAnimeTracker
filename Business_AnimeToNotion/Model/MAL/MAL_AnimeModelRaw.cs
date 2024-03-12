namespace Business_AnimeToNotion.Model.MAL
{
    public class MAL_AnimeShowRaw
    {
        public int id { get; set; }
        public string title { get; set; }
        public MAL_MainPicture main_picture { get; set; }
        public MAL_AlternativeTitle alternative_titles { get; set; }
        public decimal mean { get; set; }
        public string start_date { get; set; }
        public string media_type { get; set; }
        public List<MAL_GeneralObject> genres { get; set; } = new List<MAL_GeneralObject>();
        public List<MAL_GeneralObject> studios { get; set; } = new List<MAL_GeneralObject>();
        public int num_episodes { get; set; }
    }

    public class MAL_AlternativeTitle
    {
        public string[] synonyms { get; set; }
        public string en { get; set; }
        public string ja { get; set; }
    }

    public class MAL_MainPicture
    {
        public string medium { get; set; }
        public string large { get; set; }
    }    

    public class MAL_GeneralObject
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}

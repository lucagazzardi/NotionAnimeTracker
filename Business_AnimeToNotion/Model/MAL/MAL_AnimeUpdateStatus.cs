namespace Business_AnimeToNotion.Model.MAL
{
    public class MAL_AnimeUpdateStatus
    {
        public int anime_id { get; set; }
        public string status { get; set; }
        public int? score { get; set; }
        public string start_date { get; set; }
        public string finish_date { get; set; }   
        public string updated_at { get; set; }
        public int num_watched_episodes { get; set; }
    }
}

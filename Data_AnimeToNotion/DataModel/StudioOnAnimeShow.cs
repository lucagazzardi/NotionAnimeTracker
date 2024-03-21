namespace Data_AnimeToNotion.DataModel
{
    public class StudioOnAnimeShow
    {
        public int Id { get; set; }
        public int AnimeShowId { get; set; }
        public int StudioId { get; set; }
        public string Description { get; set; }

        public AnimeShow AnimeShow { get; set; }
        public Studio Studio { get; set; }
    }
}

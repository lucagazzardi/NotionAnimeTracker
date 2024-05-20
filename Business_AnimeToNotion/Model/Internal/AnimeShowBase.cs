using Business_AnimeToNotion.Model.Entities;

namespace Business_AnimeToNotion.Model.Internal
{
    public class AnimeShowBase
    {
        public int MalId { get; set; }
        public string NameDefault { get; set; }
        public string NameEnglish { get; set; }
        public string NameJapanese { get; set; }
        public string Cover { get; set; }
        public int? Score { get; set; }
        public DateTime? StartedAiring { get; set; }
        public string? Format { get; set; }
        public int? Episodes { get; set; }
        public bool Favorite { get; set; }
        public bool PlanToWatch { get; set; }
        public KeyValue[] Studios { get; set; }
        public KeyValue[] Genres { get; set; }
        public DateTime AddedOn { get; set; }
        public AnimeShowPersonal? Info { get; set; }
    }

    public class KeyValue

    {
        public KeyValue(int id, string value)
        {
            Id = id; Value = value;
        }

        public int Id { get; set; }
        public string Value { get; set; }
    }
}

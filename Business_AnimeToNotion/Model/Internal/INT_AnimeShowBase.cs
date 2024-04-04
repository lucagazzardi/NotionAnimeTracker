using Business_AnimeToNotion.Model.Entities;

namespace Business_AnimeToNotion.Model.Internal
{
    public class INT_AnimeShowBase
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
        public INT_KeyValue[] Studios { get; set; }
        public INT_KeyValue[] Genres { get; set; }
        public DateTime AddedOn { get; set; }
        public INT_AnimeShowPersonal? Info { get; set; }
    }

    public class INT_KeyValue
    {
        public INT_KeyValue(int id, string value)
        {
            Id = id; Value = value;
        }

        public int Id { get; set; }
        public string Value { get; set; }
    }
}

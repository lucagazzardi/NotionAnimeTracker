using Microsoft.EntityFrameworkCore;

namespace Data_AnimeToNotion.DataModel
{
    [Index(nameof(AnimeRelatedMalId))]
    public class Relation
    {
        public Guid Id { get; set; }
        public Guid AnimeShowId { get; set; }
        public int AnimeRelatedMalId { get; set; }
        public string RelationType { get; set; } 
        public string Cover { get; set; }
        
        public AnimeShow AnimeShow { get; set; }
    }
}

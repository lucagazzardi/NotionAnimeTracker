using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    public class Relation
    {
        [Column(Order = 0)]
        public Guid Id { get; set; }
        [Column(Order = 1)]
        public Guid AnimeShowId { get; set; }
        [Column(Order = 2)]
        public int AnimeRelatedMalId { get; set; }
        [Column(Order = 3)]
        public string RelationType { get; set; }  
        
        public AnimeShow AnimeShow { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    public class NotionSync
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? AnimeShowId { get; set; }
        public string NotionPageId { get; set; }
        public bool ToSync { get; set; }
        public bool MalListToSync { get; set; }
        public string Action { get; set; }
        public DateTime LastModified { get; set; }
        public string? Error { get; set; }

        public AnimeShow AnimeShow { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    [Index(nameof(NotionPageId))]
    public class NotionSync
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid? AnimeShowId { get; set; }
        public string NotionPageId { get; set; }
        public bool ToSync { get; set; }
        public string Action { get; set; }
        public DateTime LastModified { get; set; }
        public string? Error { get; set; }

        public AnimeShow AnimeShow { get; set; }
    }
}

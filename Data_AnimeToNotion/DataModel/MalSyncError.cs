
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    public class MalSyncError
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? AnimeShowId { get; set; }
        public int MalId { get; set; }  
        public string Action { get; set; }
        public string Error { get; set; }

        public AnimeShow AnimeShow { get; set; }
    }
}

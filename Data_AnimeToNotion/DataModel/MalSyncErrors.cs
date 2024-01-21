using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_AnimeToNotion.DataModel
{
    [Index(nameof(MalId))]
    public class MalSyncErrors
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid? AnimeShowId { get; set; }
        public int MalId { get; set; }  
        public string Action { get; set; }
        public string Error { get; set; }

        public AnimeShow AnimeShow { get; set; }
    }
}

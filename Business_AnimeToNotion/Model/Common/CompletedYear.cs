using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Model.Common
{
    public class CompletedYear
    {
        public Guid Id { get; set; } 
        public string NotionPageId { get; set; }
        public int Value { get; set; }
    }
}

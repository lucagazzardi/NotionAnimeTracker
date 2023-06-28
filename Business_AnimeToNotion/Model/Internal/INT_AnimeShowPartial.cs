using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Model.Entities
{
    public class INT_AnimeShowPartial
    {
        public string Title { get; set; }
        public int MalId { get; set; }
        public string Cover { get; set; }
        public INT_AnimeShowPersonal BasicInfo { get; set; }
    }
}

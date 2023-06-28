using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Model.Internal
{
    public class INT_AnimeShowFull : INT_AnimeShowBase
    {
        public List<INT_AnimeShowRelation> Relations { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Model.MAL.MAL_BasicObjects
{
    public class MAL_ApiResponseModelPartial
    {
        public MAL_NodeResponseModelPartial[] data { get; set; }
    }

    public class MAL_NodeResponseModelPartial
    {
        public MAL_AnimeShowPartial node { get; set; }
    }
}

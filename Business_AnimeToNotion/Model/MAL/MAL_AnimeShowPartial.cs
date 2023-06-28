using Business_AnimeToNotion.Model.Entities;
using Business_AnimeToNotion.Model.MAL.MAL_BasicObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Model.MAL
{

    public class MAL_AnimeShowPartial
    {
        public string title { get; set; }
        public MAL_AlternativeTitle alternative_titles { get; set; }
        public int id { get; set; }
        public MAL_MainPicture main_picture { get; set; }
    }
}

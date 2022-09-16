using System;
using System.Collections.Generic;
using System.Text;

namespace Business_AnimeToNotion.Model
{
    public class MAL_ApiResponseModel
    {
        public MAL_NodeResponseModel[] data { get; set; }
    }

    public class MAL_NodeResponseModel
    {
        public MAL_AnimeModel node { get; set; }
    }

    public class MAL_AnimeModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public MAL_MainPicture main_picture { get; set; }
        public MAL_AlternativeTitle alternative_titles { get; set; }
        public decimal mean { get; set; }
        public string start_date { get; set; }
        public string media_type { get; set; }
        public List<MAL_GeneralObject> genres { get; set; }
        public List<MAL_GeneralObject> studios { get; set; }
        public int num_episodes { get; set; } 

        //Handled fields
        public string genresJoined { get; set; }
        public string studiosJoined { get; set; }
    }

    public class MAL_MainPicture
    {
        public string medium { get; set; }
        public string large { get; set; }
    }

    public class MAL_AlternativeTitle
    {
        public string[] synonmys { get; set; }
        public string en { get; set; }
        public string ja { get; set; }
    }

    public class MAL_GeneralObject
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}

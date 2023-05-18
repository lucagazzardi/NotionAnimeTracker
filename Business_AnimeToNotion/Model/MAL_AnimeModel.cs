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
        public List<MAL_GeneralObject> genres { get; set; } = new List<MAL_GeneralObject>();
        public List<MAL_GeneralObject> studios { get; set; } = new List<MAL_GeneralObject>();
        public int num_episodes { get; set; }
        public List<MAL_RelatedShow> related_anime { get; set; } = new List<MAL_RelatedShow> ();
        public string showHidden { get; set; } = null;
    }

    public class MAL_MainPicture
    {
        public string medium { get; set; }
        public string large { get; set; }
    }

    public class MAL_AlternativeTitle
    {
        public string[] synonyms { get; set; }
        public string en { get; set; }
        public string ja { get; set; }
    }

    public class MAL_GeneralObject
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class MAL_RelatedShow
    {
        public MAL_RelatedAnime_Node node { get; set; }
        public string relation_type { get; set; }
        public string relation_type_formatter { get; set; }
    }

    public class MAL_RelatedAnime_Node
    {
        public int id { get; set; }
        public string title { get; set; }
        public MAL_MainPicture main_picture { get; set; }
    }
}

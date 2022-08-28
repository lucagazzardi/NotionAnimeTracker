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
        public DateTime start_date { get; set; }
        public string media_type { get; set; }
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
}

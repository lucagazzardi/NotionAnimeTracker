using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Business_AnimeToNotion.HTTPClient
{    
    public static class StaticHttpClient
    {
        public static readonly HttpClient MALHttpClient = new HttpClient();
    }
}

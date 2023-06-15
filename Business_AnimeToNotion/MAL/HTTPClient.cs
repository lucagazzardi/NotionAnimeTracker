using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.MAL
{
    public static class StaticHttpClient
    {
        public static readonly HttpClient MALHttpClient = new HttpClient();
    }
}

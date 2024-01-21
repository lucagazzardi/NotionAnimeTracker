using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Model.Auth
{
    public class ResponseTokens
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}

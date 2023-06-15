using System;
using System.Collections.Generic;
using System.Text;

namespace Business_AnimeToNotion.Main_Integration.Exceptions
{
    public class MAL_Exception : Exception
    {
        public MAL_Exception(string customMessage) : base(customMessage) { }
    }
}

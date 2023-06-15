using System;
using System.Collections.Generic;
using System.Text;

namespace Business_AnimeToNotion.Main_Integration.Exceptions
{
    public class Notion_Exception : Exception
    {
        public Notion_Exception(string customMessage) : base(customMessage) { }
    }
}
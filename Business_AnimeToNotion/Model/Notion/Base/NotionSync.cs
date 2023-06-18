using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Model.Notion.Base
{
    public class NotionSync
    {
        public OperationType Type { get; set; }
        public NotionBaseObject NotionBaseObject { get; set; }
    }
}

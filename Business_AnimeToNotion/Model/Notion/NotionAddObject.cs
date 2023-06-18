using Business_AnimeToNotion.Model.Notion.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Model.Notion
{
    public class NotionAddObject : NotionBaseObject
    {
        public string NameEnglish { get; set; }
        public string NameOriginal { get; set; }
        public int MalScore { get; set; }
        public DateTime StartedAiring { get; set; }
        public string Cover { get; set; }
        public int Episodes { get; set; }
        public int MalId { get; set; }
        public List<NotionAddKeyValue> Genres { get; set; }
        public List<NotionAddKeyValue> Studios { get; set; }
        public NotionEditObject NotionEditObject { get; set; }
    }

    public class NotionAddKeyValue
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Business_AnimeToNotion.Model.Notion.Base
{
    public class NotionSync
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public OperationType Type { get; set; }
    }

    public class NotionSyncAdd : NotionSync
    {
        public NotionAddObject NotionAddObject { get; set; }
    }

    public class NotionSyncEdit : NotionSync
    {
        public NotionEditObject NotionEditObject { get; set; }

        public string Title { get; set; }
        public int MalId { get; set; }
    }

    public class NotionSyncRemove : NotionSync
    {
        public NotionRemoveObject NotionRemoveObject { get; set; }

        public string Title { get; set; }
        public int MalId { get; set; }
    }
}

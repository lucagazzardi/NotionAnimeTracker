using Business_AnimeToNotion.Model.Notion.Base;

namespace Business_AnimeToNotion.Integrations.Notion
{
    public interface INotion_Integration
    {
        void SendSyncToNotion(NotionSync notionSync);
    }
}

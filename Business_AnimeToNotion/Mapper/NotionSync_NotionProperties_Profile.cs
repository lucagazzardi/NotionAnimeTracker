using AutoMapper;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.Notion;
using Data_AnimeToNotion.DataModel;
using Microsoft.Data.SqlClient;
using Notion.Client;

namespace Business_AnimeToNotion.Mapper
{
    public class NotionSync_NotionProperties_Profile : Profile
    {
        public NotionSync_NotionProperties_Profile()
        {
            CreateMap<NotionAddObject, Dictionary<string, PropertyValue>>().ConvertUsing((source, dst) =>
            {
                var result = new Dictionary<string, PropertyValue>()
                {
                    { "Name English", Mapping.Mapper.Map<TitlePropertyValue>(source.NameEnglish) },
                    { "Name Original", Mapping.Mapper.Map<RichTextPropertyValue>(source.NameOriginal) },
                    { "Cover", Mapping.Mapper.Map<FilesPropertyValue>(source.Cover) },
                    { "MAL Score", Mapping.Mapper.Map<NumberPropertyValue>(source.MalScore) },
                    { "MAL Id", Mapping.Mapper.Map<NumberPropertyValue>(source.MalId) },
                    { "MAL Link", Mapping.Mapper.Map<UrlPropertyValue>(source.MalId) },
                    { "Episodes", Mapping.Mapper.Map<NumberPropertyValue>(source.Episodes) },
                    { "Studios", Mapping.Mapper.Map<RichTextPropertyValue>(source.Studios) },
                    { "Genres", Mapping.Mapper.Map<RichTextPropertyValue>(source.Genres) }
                };

                if (source.Format != null)
                    result.Add("Format", Mapping.Mapper.Map<SelectPropertyValue>(source.Format));

                if (source.StartedAiring != null)
                    result.Add("Started Airing", Mapping.Mapper.Map<DatePropertyValue>(source.StartedAiring.ToString()));

                if (source.NotionEditObject != null)
                {
                    foreach(var item in Mapping.Mapper.Map<Dictionary<string, PropertyValue>>(source.NotionEditObject))
                    {
                        result.Add(item.Key, item.Value);
                    }
                }

                return result;
            });

            CreateMap<NotionEditObject, Dictionary<string, PropertyValue>>().ConvertUsing((source, dst) =>
            {
                Dictionary<string, PropertyValue> properties = new Dictionary<string, PropertyValue>();

                if (source.Status != null)
                    properties.Add("Status", Mapping.Mapper.Map<SelectPropertyValue>(source.Status));

                if (source.PersonalScore != 0)
                    properties.Add("Personal Score", Mapping.Mapper.Map<NumberPropertyValue>(source.PersonalScore));

                if (source.StartedOn != DateTime.MinValue)
                    properties.Add("Started On", Mapping.Mapper.Map<DatePropertyValue>(source.StartedOn.ToString()));

                if (source.FinishedOn != DateTime.MinValue)
                    properties.Add("Finished On", Mapping.Mapper.Map<DatePropertyValue>(source.FinishedOn.ToString()));

                if (source.Notes != null)
                    properties.Add("Notes", Mapping.Mapper.Map<RichTextPropertyValue>(source.Notes));

                if (source.CompletedYear != null)
                    properties.Add("Completed Year", Mapping.Mapper.Map<RelationPropertyValue>(source.CompletedYear.NotionPageId));

                return properties;
            });
        }
    }
}

using AutoMapper;
using Business_AnimeToNotion.Functions.Static;
using Business_AnimeToNotion.Model.MAL;
using Business_AnimeToNotion.Model.Notion;
using Data_AnimeToNotion.DataModel;
using Newtonsoft.Json;
using Notion.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Mapper
{
    public class String_NotionProperty_Reverse_Profile : Profile
    {
        public String_NotionProperty_Reverse_Profile()
        {
            //Name English
            CreateMap<string, TitlePropertyValue>().ForMember(dto => dto.Title, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = source } } }));

            //Name Original
            CreateMap<string, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = source } } }));

            //MAL Score
            CreateMap<int, NumberPropertyValue>().ForMember(dto => dto.Number, map => map.MapFrom(source => Convert.ToDouble(source)));

            //Next To Watch
            CreateMap<bool, CheckboxPropertyValue>().ForMember(dto => dto.Checkbox, map => map.MapFrom(source => source));

            //Format
            CreateMap<string, SelectPropertyValue>().ForMember(dto => dto.Select, map => map.MapFrom(source => new SelectOption() { Name = source }));

            //Status
            CreateMap<string, SelectPropertyValue>().ForMember(dto => dto.Select, map => map.MapFrom(source => new SelectOption() { Name = source }));

            //Started Airing
            CreateMap<string, DatePropertyValue>().ConvertUsing((source, dest) =>
            {
                DateTime result = DateTime.Now;
                if (DateTime.TryParse(source, out result))
                    return new DatePropertyValue() { Date = new Date() { Start = DateTime.Parse(source) } };
                else
                    return new DatePropertyValue() { Date = new Date() { Start = null } };
            });

            //Cover
            CreateMap<string, FilesPropertyValue>().ForMember(dto => dto.Files, map => map.MapFrom(source => new List<FileObjectWithName>() { new ExternalFileWithName() { Type = "external", Name = source, External = new ExternalFileWithName.Info() { Url = source } } }));

            //MAL Id
            CreateMap<int, NumberPropertyValue>().ForMember(dto => dto.Number, map => map.MapFrom(source => source));

            //MAL Link
            CreateMap<int, UrlPropertyValue>().ForMember(dto => dto.Url, map => map.MapFrom(source => Common_Utilities.Property_MAL_Link(source.ToString())));

            //Episodes
            CreateMap<int, NumberPropertyValue>().ForMember(dto => dto.Number, map => map.MapFrom(source => source));

            //Studios
            CreateMap<List<MAL_GeneralObject>, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = string.Join(", ", source.Select(x => x.name)) } } }));
            CreateMap<List<NotionAddKeyValue>, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = string.Join(", ", source.Select(x => x.Value)) } } }));

            //Genres
            CreateMap<List<MAL_GeneralObject>, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = string.Join(", ", source.Select(x => x.name)) } } }));
            CreateMap<List<NotionAddKeyValue>, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = string.Join(", ", source.Select(x => x.Value)) } } }));

            //Show Hidden
            CreateMap<string, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = source } } }));

            //Completed Year
            CreateMap<string, RelationPropertyValue>().ForMember(dto => dto.Relation, map => map.MapFrom(source => new List<ObjectId>() { new ObjectId() { Id = source } }));



            // Reverse Maps
            CreateMap<PropertyValue, string>().ConvertUsing((src, dest) =>
            {
                switch (src.Type)
                {
                    case PropertyValueType.Title:
                        // Need to serialize to JSON the object and deserialize in the correct type
                        var serializedTitle = JsonConvert.SerializeObject(((TitlePropertyValue)src).Title[0]);
                        var deserializedTitle = JsonConvert.DeserializeObject<RichTextText>(serializedTitle);
                        return deserializedTitle.Text.Content;
                    case PropertyValueType.RichText:
                        // Need to serialize to JSON the object and deserialize in the correct type
                        var serializedText = JsonConvert.SerializeObject(((RichTextPropertyValue)src).RichText[0]);
                        var deserializedText = JsonConvert.DeserializeObject<RichTextText>(serializedText);
                        return deserializedText.Text.Content;
                    case PropertyValueType.Number:
                        return ((NumberPropertyValue)src).Number != null ? ((NumberPropertyValue)src).Number.Value.ToString() : "0";
                    case PropertyValueType.Checkbox:
                        return ((CheckboxPropertyValue)src).Checkbox.ToString();
                    case PropertyValueType.Select:
                        return ((SelectPropertyValue)src).Select?.Name;
                    case PropertyValueType.Date:
                        return ((DatePropertyValue)src).Date != null ? !string.IsNullOrEmpty(((DatePropertyValue)src).Date.Start.ToString()) ? ((DatePropertyValue)src).Date.Start.ToString() : null : null;
                    case PropertyValueType.Files:
                        return ((FilesPropertyValue)src).Files.First().Name;
                    case PropertyValueType.Url:
                        return ((UrlPropertyValue)src).Url;
                    default: return "";
                }
            });
        }
    }
}

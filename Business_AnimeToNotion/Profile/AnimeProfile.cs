using AutoMapper;
using Business_AnimeToNotion.Common;
using Business_AnimeToNotion.Main_Integration;
using Business_AnimeToNotion.Model;
using Business_AnimeToNotion.Profile.AutoMapperConfig;
using Newtonsoft.Json;
using Notion.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business_AnimeToNotion.Profile
{
    public class AnimeProfile : AutoMapper.Profile
    {
        public AnimeProfile()
        {
            CreateMap<MAL_AnimeModel, Dictionary<string, PropertyValue>>().ConvertUsing((source,dst) => {
                return new Dictionary<string, PropertyValue>
                {
                    { Notion_Properties_Mapping.Name_English, MappingHandler.Mapper.Map<TitlePropertyValue>(string.IsNullOrEmpty(source.alternative_titles.en) ? source.title : source.alternative_titles.en) },
                    { Notion_Properties_Mapping.Name_Original, MappingHandler.Mapper.Map<RichTextPropertyValue>(source.title) },
                    { Notion_Properties_Mapping.MAL_Rating, MappingHandler.Mapper.Map<NumberPropertyValue>(source.mean) },
                    { Notion_Properties_Mapping.Next_To_Watch, MappingHandler.Mapper.Map<CheckboxPropertyValue>(false) },
                    { Notion_Properties_Mapping.Distributor, MappingHandler.Mapper.Map<MultiSelectPropertyValue>(Notion_Properties_Mapping.AnimeSaturn) },
                    { Notion_Properties_Mapping.Type, MappingHandler.Mapper.Map<SelectPropertyValue>(MAL_NotionUtility.Property_Type(source.media_type)) },
                    { Notion_Properties_Mapping.Watched, MappingHandler.Mapper.Map<SelectPropertyValue>(Notion_Properties_Mapping.ToWatch) },
                    { Notion_Properties_Mapping.Started_Airing, MappingHandler.Mapper.Map<DatePropertyValue>(source.start_date) },
                    { Notion_Properties_Mapping.Cover, MappingHandler.Mapper.Map<FilesPropertyValue>(source.main_picture.medium) },
                    { Notion_Properties_Mapping.MAL_Id, MappingHandler.Mapper.Map<NumberPropertyValue>(source.id) },
                    { Notion_Properties_Mapping.MAL_Link, MappingHandler.Mapper.Map<UrlPropertyValue>(source.id) },
                    { Notion_Properties_Mapping.Episodes, MappingHandler.Mapper.Map<NumberPropertyValue>(source.num_episodes) },
                    { Notion_Properties_Mapping.Studios, MappingHandler.Mapper.Map<RichTextPropertyValue>(source.studios) },
                    { Notion_Properties_Mapping.Genres, MappingHandler.Mapper.Map<RichTextPropertyValue>(source.genres) },
                    { Notion_Properties_Mapping.Show_Hidden, MappingHandler.Mapper.Map<RichTextPropertyValue>(source.showHidden) }
                };
            });



            //Name English
            CreateMap<string, TitlePropertyValue>().ForMember(dto => dto.Title, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = source } } }));

            //Name Original
            CreateMap<string, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = source } } }));

            //MAL Rating
            CreateMap<decimal, NumberPropertyValue>().ForMember(dto => dto.Number, map => map.MapFrom(source => Double.Parse(source.ToString())));

            //Next To Watch
            CreateMap<bool, CheckboxPropertyValue>().ForMember(dto => dto.Checkbox, map => map.MapFrom(source => false));

            //Distributor
            CreateMap<string, MultiSelectPropertyValue>().ForMember(dto => dto.MultiSelect, map => map.MapFrom(source => new List<SelectOption>() { new SelectOption() { Name = Notion_Properties_Mapping.AnimeSaturn } }));

            //Type
            CreateMap<string, SelectPropertyValue>().ForMember(dto => dto.Select, map => map.MapFrom(source => new SelectOption() { Name = source }));

            //Watched
            CreateMap<string, SelectPropertyValue>().ForMember(dto => dto.Select, map => map.MapFrom(source => new SelectOption() { Name = source }));

            //Started Airing
            CreateMap<string, DatePropertyValue>().ForMember(dto => dto.Date, map => map.MapFrom(source => new Date() { Start = DateTime.Parse(source) }));

            //Cover
            CreateMap<string, FilesPropertyValue>().ForMember(dto => dto.Files, map => map.MapFrom(source => new List<FileObjectWithName>() { new ExternalFileWithName() { Type = Notion_Properties_Mapping.External, Name = source, External = new ExternalFileWithName.Info() { Url = source } } }));

            //MAL Id
            CreateMap<int, NumberPropertyValue>().ForMember(dto => dto.Number, map => map.MapFrom(source => source));

            //MAL Link
            CreateMap<int, UrlPropertyValue>().ForMember(dto => dto.Url, map => map.MapFrom(source => MAL_NotionUtility.Property_MAL_Link(source.ToString())));

            //Episodes
            CreateMap<int, NumberPropertyValue>().ForMember(dto => dto.Number, map => map.MapFrom(source => source));

            //Studios
            CreateMap<List<MAL_GeneralObject>, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = string.Join(", ", source.Select(x => x.name)) } } }));

            //Genres
            CreateMap<List<MAL_GeneralObject>, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = string.Join(", ", source.Select(x => x.name)) } } }));

            //Show Hidden
            CreateMap<string, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = source ?? string.Empty } } }));



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
                        return ((SelectPropertyValue)src).Select.Name;
                    case PropertyValueType.Date:
                        return ((DatePropertyValue)src).Date != null ? ((DatePropertyValue)src).Date.ToString() : null;
                    case PropertyValueType.Files:
                        return ((FilesPropertyValue)src).Files.First().Name;
                    case PropertyValueType.Url:
                        return ((UrlPropertyValue)src).Url;
                    default: return "";
                }
            });

            // Filter out the useless fields
            CreateMap<Dictionary<string, PropertyValue>, CompareAnimeModel>().ConvertUsing((src, dest) =>
            {
                var usefullFields = new List<string>() 
                {
                    Notion_Properties_Mapping.Name_English,
                    Notion_Properties_Mapping.Name_Original,
                    Notion_Properties_Mapping.MAL_Rating,
                    Notion_Properties_Mapping.Type,
                    Notion_Properties_Mapping.Started_Airing,
                    Notion_Properties_Mapping.Cover,
                    Notion_Properties_Mapping.MAL_Id,
                    Notion_Properties_Mapping.MAL_Link,
                    Notion_Properties_Mapping.Episodes,
                    Notion_Properties_Mapping.Studios,
                    Notion_Properties_Mapping.Genres
                };

                return new CompareAnimeModel() { CompareProperties = src.Where(x => usefullFields.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value) };             
            });
        }
    }
}

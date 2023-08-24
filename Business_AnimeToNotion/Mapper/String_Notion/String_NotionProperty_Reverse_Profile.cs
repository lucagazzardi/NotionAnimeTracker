using AutoMapper;
using Business_AnimeToNotion.Functions.Static;
using Notion.Client;

namespace Business_AnimeToNotion.Mapper.String_Notion
{
    public class String_NotionProperty_Reverse_Profile : Profile
    {
        public String_NotionProperty_Reverse_Profile()
        {
            //Name English
            CreateMap<string, TitlePropertyValue>().ForMember(dto => dto.Title, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = source } } }));

            //Name Original
            //Genres
            //Studio
            CreateMap<string, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = source } } }));

            //MAL Score
            //MAL Id
            //Episodes
            CreateMap<int, NumberPropertyValue>().ForMember(dto => dto.Number, map => map.MapFrom(source => source != 0 ? source : (double?)null));

            //Next To Watch
            CreateMap<bool, CheckboxPropertyValue>().ForMember(dto => dto.Checkbox, map => map.MapFrom(source => source));

            //Format
            //Status
            CreateMap<string, SelectPropertyValue>().ForMember(dto => dto.Select, map => map.MapFrom(source => new SelectOption() { Name = source }));

            //Started Airing
            CreateMap<DateTime, DatePropertyValue>().ForMember(dto => dto.Date, map => map.MapFrom(source => new Date() { Start = source != DateTime.MinValue ? source : null }));

            //Cover
            CreateMap<string, FilesPropertyValue>().ForMember(dto => dto.Files, map => map.MapFrom(source => new List<FileObjectWithName>() { new ExternalFileWithName() { Type = "external", Name = source, External = new ExternalFileWithName.Info() { Url = source } } }));

            //MAL Link
            CreateMap<int, UrlPropertyValue>().ForMember(dto => dto.Url, map => map.MapFrom(source => Common_Utilities.Property_MAL_Link(source.ToString())));

            //Completed Year
            CreateMap<string, RelationPropertyValue>().ForMember(dto => dto.Relation, map => map.MapFrom(source => new List<ObjectId>() { new ObjectId() { Id = source } }));

        }
    }
}

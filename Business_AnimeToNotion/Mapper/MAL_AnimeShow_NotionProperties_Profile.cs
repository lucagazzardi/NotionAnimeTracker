using AutoMapper;
using Business_AnimeToNotion.Functions.Static;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.MAL;
using Business_AnimeToNotion.Model.SyncMAL;
using Newtonsoft.Json;
using Notion.Client;

namespace Business_AnimeToNotion.Mapper
{
    public class MAL_AnimeShow_NotionProperties_Profile : Profile
    {
        public MAL_AnimeShow_NotionProperties_Profile()
        {
            CreateMap<MAL_AnimeShowRaw, Dictionary<string, PropertyValue>>().ConvertUsing((source, dst) =>
            {
                return new Dictionary<string, PropertyValue>
                {
                    { "Name English", Mapping.Mapper.Map<TitlePropertyValue>(string.IsNullOrEmpty(source.alternative_titles.en) ? source.title : source.alternative_titles.en) },
                    { "Name Original", Mapping.Mapper.Map<RichTextPropertyValue>(source.title) },
                    { "MAL Score", Mapping.Mapper.Map<NumberPropertyValue>(Convert.ToInt32(Math.Round(source.mean * 10))) },
                    { "Next To Watch", Mapping.Mapper.Map<CheckboxPropertyValue>(false) },
                    { "Format", Mapping.Mapper.Map<SelectPropertyValue>(Common_Utilities.MALFormatToCommon(source.media_type)) },
                    { "Status", Mapping.Mapper.Map<SelectPropertyValue>("To Watch") },
                    { "Started Airing", Mapping.Mapper.Map<DatePropertyValue>(source.start_date) },
                    { "Cover", Mapping.Mapper.Map<FilesPropertyValue>(source.main_picture.medium) },
                    { "MAL Id", Mapping.Mapper.Map<NumberPropertyValue>(source.id) },
                    { "MAL Link", Mapping.Mapper.Map<UrlPropertyValue>(source.id) },
                    { "Episodes", Mapping.Mapper.Map<NumberPropertyValue>(source.num_episodes) },
                    { "Studios", Mapping.Mapper.Map<RichTextPropertyValue>(source.studios) },
                    { "Genres", Mapping.Mapper.Map<RichTextPropertyValue>(source.genres) }
                };
            });   

            // Filter out the useless fields
            CreateMap<Dictionary<string, PropertyValue>, Compare_AnimeModel>().ConvertUsing((src, dest) =>
            {
                var usefullFields = new List<string>()
                {
                    "Name English",
                    "Name Original",
                    "MAL Score",
                    "Format",
                    "Started Airing",
                    "Cover",
                    "MAL Id",
                    "MAL Link",
                    "Episodes",
                    "Studios",
                    "Genres"
                };

                return new Compare_AnimeModel() { CompareProperties = src.Where(x => usefullFields.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value) };
            });

        }
    }
}

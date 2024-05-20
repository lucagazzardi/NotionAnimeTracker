using AutoMapper;
using Business_AnimeToNotion.Mapper.Config;
using Data_AnimeToNotion.DataModel;
using Notion.Client;

namespace Business_AnimeToNotion.Mapper.Entity_Notion
{
    public class Entity_Notion_Profile : Profile
    {
        public Entity_Notion_Profile()
        {
            CreateMap<AnimeShow, Dictionary<string, PropertyValue>>().ConvertUsing((source, dst) =>
            {
                var result = new Dictionary<string, PropertyValue>()
                {
                    { "Name English", Mapping.Mapper.Map<TitlePropertyValue>(source.NameEnglish) },
                    { "Name Original", Mapping.Mapper.Map<RichTextPropertyValue>(source.NameDefault) },
                    { "Next To Watch", Mapping.Mapper.Map<CheckboxPropertyValue>(source.PlanToWatch) },
                    { "Format", Mapping.Mapper.Map<SelectPropertyValue>(source.Format ?? "TV") },
                    { "Status", Mapping.Mapper.Map<SelectPropertyValue>(source.Status) },
                    { "Cover", Mapping.Mapper.Map<FilesPropertyValue>(source.Cover) },
                    { "Favorite", Mapping.Mapper.Map<CheckboxPropertyValue>(source.Favorite) },
                    { "Genres", Mapping.Mapper.Map<RichTextPropertyValue>(string.Join(", ", source.GenreOnAnimeShows.Select(x => x.Description))) },
                    { "MAL Id", Mapping.Mapper.Map<NumberPropertyValue>(source.MalId) },
                    { "MAL Link", Mapping.Mapper.Map<UrlPropertyValue>(source.MalId) },
                    { "Studios", Mapping.Mapper.Map<RichTextPropertyValue>(string.Join(", ", source.StudioOnAnimeShows.Select(x => x.Description))) }
                };

                if (source.Episodes != null)
                    result.Add("Episodes", Mapping.Mapper.Map<NumberPropertyValue>(source.Episodes));

                if (source.Score != null)
                    result.Add("MAL Score", Mapping.Mapper.Map<NumberPropertyValue>(source.Score));

                if (source.AnimeShowProgress.EpisodesProgress != null)
                    result.Add("Episodes Progress", Mapping.Mapper.Map<NumberPropertyValue>(source.AnimeShowProgress.EpisodesProgress));

                if (source.AnimeShowProgress.PersonalScore != null)
                    result.Add("Personal Score", Mapping.Mapper.Map<NumberPropertyValue>(source.AnimeShowProgress.PersonalScore));

                if (source.AnimeShowProgress.StartedOn != null)
                    result.Add("Started On", Mapping.Mapper.Map<DatePropertyValue>(source.AnimeShowProgress.StartedOn));

                if (source.AnimeShowProgress.FinishedOn != null)
                    result.Add("Finished On", Mapping.Mapper.Map<DatePropertyValue>(source.AnimeShowProgress.FinishedOn));

                if (source.StartedAiring != null)
                    result.Add("Started Airing", Mapping.Mapper.Map<DatePropertyValue>(source.StartedAiring));

                if(source.AnimeShowProgress.Notes != null)
                    result.Add("Notes", Mapping.Mapper.Map<RichTextPropertyValue>(source.AnimeShowProgress.Notes));

                return result;
            });

        }
    }
}

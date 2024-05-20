using AutoMapper;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.MAL;

namespace Business_AnimeToNotion.Mapper.MAL_Internal
{
    public class MAL_Internal_Profile : Profile
    {
        public MAL_Internal_Profile()
        {
            CreateMap<AnimeShowRaw, AnimeShowFull>()
                .ForMember(dto => dto.NameDefault, map => map.MapFrom(source => source.title))
                .ForMember(dto => dto.NameEnglish, map => map.MapFrom(source =>
                    !string.IsNullOrEmpty(source.alternative_titles.en) ?
                    source.alternative_titles.en :
                    source.title))
                .ForMember(dto => dto.NameJapanese, map => map.MapFrom(source =>
                    !string.IsNullOrEmpty(source.alternative_titles.ja) ?
                    source.alternative_titles.ja :
                    source.title))
                .ForMember(dto => dto.Cover, map => map.MapFrom(source => source.main_picture.large))
                .ForMember(dto => dto.Score, map => map.MapFrom(source => source.mean != 0 ? Convert.ToInt32(Math.Round(source.mean * 10)) : (int?)null))
                .ForMember(dto => dto.StartedAiring, map => map.MapFrom(source => Mapping.Mapper.Map<DateTime?>(source.start_date)))
                .ForMember(dto => dto.Episodes, map => map.MapFrom(source => source.num_episodes != 0 ? source.num_episodes : (int?)null))
                .ForMember(dto => dto.Studios, map => map.MapFrom(source => source.studios.Select(x => new KeyValue((int)x.id, x.name))))
                .ForMember(dto => dto.Genres, map => map.MapFrom(source => source.genres.Select(x => new KeyValue((int)x.id, x.name))));

            CreateMap<string, DateTime?>().ConvertUsing((src, dest) =>
            {
                DateTime date;
                if (DateTime.TryParse(src, out date))
                    return date;
                else
                    return null;
            });
        }
    }
}

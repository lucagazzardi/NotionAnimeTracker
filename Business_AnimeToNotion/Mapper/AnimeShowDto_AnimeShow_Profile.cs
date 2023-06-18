using AutoMapper;
using Business_AnimeToNotion.Mapper.Config;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Model;

namespace Business_AnimeToNotion.Mapper
{
    public class AnimeShowDto_AnimeShow_Profile : Profile
    {
        public AnimeShowDto_AnimeShow_Profile()
        {
            CreateMap<AnimeShowDto, AnimeShow>();

            CreateMap<RelationDto, Data_AnimeToNotion.DataModel.Relation>()
                .ForMember(dto => dto.Id, map => map.MapFrom(source => Guid.NewGuid()));

            CreateMap<WatchingTimeDto, WatchingTime>()
                .ForMember(dto => dto.Id, map => map.MapFrom(source => Guid.NewGuid()));

            CreateMap<ScoreDto, Score>()
                .ForMember(dto => dto.Id, map => map.MapFrom(source => Guid.NewGuid()));

            CreateMap<NoteDto, Note>()
                .ForMember(dto => dto.Id, map => map.MapFrom(source => Guid.NewGuid()));

            CreateMap<IEnumerable<GenreOnAnimeShow>, Dictionary<int, string>>().ConvertUsing((src, dest) =>
            {
                dest = new Dictionary<int, string>();
                foreach (var genreOnShow in src)
                {
                    dest.Add(genreOnShow.Genre.MalId, genreOnShow.Genre.Description);
                }
                return dest;
            });

            CreateMap<IEnumerable<StudioOnAnimeShow>, Dictionary<int, string>>().ConvertUsing((src, dest) =>
            {
                dest = new Dictionary<int, string>();
                foreach (var studioOnShow in src)
                {
                    dest.Add(studioOnShow.Studio.MalId, studioOnShow.Studio.Description);
                }
                return dest;
            });

            CreateMap<IEnumerable<Relation>, List<RelationDto>>().ConvertUsing((src, dest) =>
            {
                dest = new List<RelationDto>();
                foreach (var relation in src)
                {
                    Mapping.Mapper.Map<RelationDto>(relation);
                }
                return dest;
            });


            CreateMap<AnimeShow, AnimeShowDto>()
                .ForMember(dto => dto.Score, map => map.MapFrom(source => Mapping.Mapper.Map<Score>(source.Score)))
                .ForMember(dto => dto.WatchingTime, map => map.MapFrom(source => Mapping.Mapper.Map<WatchingTime>(source.WatchingTime)))
                .ForMember(dto => dto.Note, map => map.MapFrom(source => Mapping.Mapper.Map<Note>(source.Note)))
                .ForMember(dto => dto.Genres, map => map.MapFrom(source => Mapping.Mapper.Map<Dictionary<int, string>>(source.GenreOnAnimeShows)))
                .ForMember(dto => dto.Studios, map => map.MapFrom(source => Mapping.Mapper.Map<Dictionary<int, string>>(source.StudioOnAnimeShows)))
                .ForMember(dto => dto.Relations, map => map.MapFrom(source => Mapping.Mapper.Map<List<RelationDto>>(source.Relations)));

        }
    }
}

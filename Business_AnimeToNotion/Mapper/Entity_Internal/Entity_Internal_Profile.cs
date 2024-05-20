using AutoMapper;
using Business_AnimeToNotion.Model.Entities;
using Business_AnimeToNotion.Model.Internal;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.Mapper.Entity_Internal
{
    public class Entity_Internal_Profile : Profile
    {
        public Entity_Internal_Profile()
        {
            CreateMap<AnimeShow, AnimeShowFull>()
                .ForMember(dto => dto.Score, map => map.MapFrom(source => source.Score))
                .ForMember(dto => dto.Studios, map => map.MapFrom(source => source.StudioOnAnimeShows != null ? source.StudioOnAnimeShows
                    .Select(x => new KeyValue(x.StudioId, x.Description)) : null))
                .ForMember(dto => dto.Genres, map => map.MapFrom(source => source.GenreOnAnimeShows != null ? source.GenreOnAnimeShows
                    .Select(x => new KeyValue(x.GenreId, x.Description)) : null))
                .ForMember(dto => dto.Edit, map => map.MapFrom(source => new AnimeShowEdit()
                {
                    Id = source.Id,
                    Status = source.Status,
                    EpisodesProgress = source.AnimeShowProgress.EpisodesProgress,
                    PersonalScore = source.AnimeShowProgress.PersonalScore,
                    StartedOn = source.AnimeShowProgress.StartedOn,
                    FinishedOn = source.AnimeShowProgress.FinishedOn,
                    Notes = source.AnimeShowProgress.Notes,
                    CompletedYear = source.AnimeShowProgress.CompletedYear
                }))
                .ForMember(dto => dto.Info, map => map.MapFrom(source => new AnimeShowPersonal() { Id = source.Id, Status = source.Status }));

        }
    }
}

using AutoMapper;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.Common;
using Business_AnimeToNotion.Model.Entities;
using Business_AnimeToNotion.Model.Internal;
using Data_AnimeToNotion.DataModel;
using System.Security.Cryptography.X509Certificates;

namespace Business_AnimeToNotion.Mapper.Entity_Internal
{
    public class Entity_Internal_Profile : Profile
    {
        public Entity_Internal_Profile()
        {
            CreateMap<AnimeShow, INT_AnimeShowFull>()
                .ForMember(dto => dto.Score, map => map.MapFrom(source => source.Score != null ? source.Score.MalScore : (int?)null))
                .ForMember(dto => dto.Studios, map => map.MapFrom(source => source.StudioOnAnimeShows != null ? source.StudioOnAnimeShows
                    .Select(x => new INT_KeyValue(x.Studio.MalId, x.Studio.Description)) : null))
                .ForMember(dto => dto.Genres, map => map.MapFrom(source => source.GenreOnAnimeShows != null ? source.GenreOnAnimeShows
                    .Select(x => new INT_KeyValue(x.Genre.MalId, x.Genre.Description)) : null))
                .ForMember(dto => dto.Relations, map => map.MapFrom(source => source.Relations
                    .Select(x => new INT_AnimeShowRelation()
                    {
                        RelatedMalId = x.AnimeRelatedMalId,
                        Type = x.RelationType,
                        Cover = x.Cover
                    }).ToList()))
                .ForMember(dto => dto.Edit, map => map.MapFrom(source => new INT_AnimeShowEdit()
                {
                    Id = source.Id,
                    Status = source.Status,
                    PersonalScore = source.Score != null && source.Score.PersonalScore != null ? source.Score.PersonalScore : null,
                    StartedOn = source.WatchingTime != null ? source.WatchingTime.StartedOn : (DateTime?)null,
                    FinishedOn = source.WatchingTime != null ? source.WatchingTime.FinishedOn : (DateTime?)null,
                    Notes = source.Note != null ? source.Note.Notes : null,
                    CompletedYear = source.WatchingTime != null && source.WatchingTime.CompletedYear != null ? source.WatchingTime.CompletedYear : null
                }))
                .ForMember(dto => dto.Info, map => map.MapFrom(source => new INT_AnimeShowPersonal() { Id = source.Id, Status = source.Status }));

            CreateMap<Year, CompletedYear>()
                .ForMember(dto => dto.Id, map => map.MapFrom(source => source.Id))
                .ForMember(dto => dto.NotionPageId, map => map.MapFrom(source => source.NotionPageId))
                .ForMember(dto => dto.Value, map => map.MapFrom(source => source.YearValue));
            
        }
    }
}

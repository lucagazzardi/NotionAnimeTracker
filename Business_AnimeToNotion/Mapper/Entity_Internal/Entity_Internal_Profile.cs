using AutoMapper;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.Common;
using Business_AnimeToNotion.Model.Internal;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.Mapper.Entity_Internal
{
    public class Entity_Internal_Profile : Profile
    {
        public Entity_Internal_Profile()
        {
            CreateMap<AnimeShow, INT_AnimeShowFull>()
                .ForMember(dto => dto.Score, map => map.MapFrom(source => source.Score != null ? source.Score.MalScore : (int?)null))
                .ForMember(dto => dto.Score, map => map.MapFrom(source => source.Score != null ? source.Score.MalScore : (int?)null))
                .ForMember(dto => dto.Relations, map => map.MapFrom(source => source.Relations
                    .Select(x => new INT_AnimeShowRelation()
                    {
                        RelatedMalId = x.AnimeRelatedMalId,
                        Type = x.RelationType,
                        Cover = x.Cover
                    }).ToList()))
                .ForMember(dto => dto.Edit, map => map.MapFrom(source => Mapping.Mapper.Map<INT_AnimeShowEdit>(source)));


            CreateMap<AnimeShow, INT_AnimeShowEdit>()
                .ForMember(dto => dto.Id, map => map.MapFrom(source => source.Id))
                .ForMember(dto => dto.Status, map => map.MapFrom(source => source.Status))
                .ForMember(dto => dto.PlanToWatch, map => map.MapFrom(source => source.PlanToWatch))
                .ForMember(dto => dto.PersonalScore, map => map.MapFrom(source => source.Score != null ? source.Score.PersonalScore : null))
                .ForMember(dto => dto.StartedOn, map => map.MapFrom(source => source.WatchingTime != null ? source.WatchingTime.StartedOn : (DateTime?)null))
                .ForMember(dto => dto.FinishedOn, map => map.MapFrom(source => source.WatchingTime != null ? source.WatchingTime.FinishedOn : (DateTime?)null))
                .ForMember(dto => dto.Favorite, map => map.MapFrom(source => source.Score != null ? source.Score.Favorite : false))
                .ForMember(dto => dto.Notes, map => map.MapFrom(source => source.Note != null ? source.Note.Notes : null))
                .ForMember(dto => dto.CompletedYear, map => map.MapFrom(source => source.WatchingTime != null && source.WatchingTime.CompletedYear != null ?
                    new CompletedYear()
                    { 
                        Id = source.WatchingTime.CompletedYear.Value, 
                        NotionPageId = source.WatchingTime.Year.NotionPageId, 
                        Value = source.WatchingTime.Year.YearValue 
                    } : null));                
        }
    }
}

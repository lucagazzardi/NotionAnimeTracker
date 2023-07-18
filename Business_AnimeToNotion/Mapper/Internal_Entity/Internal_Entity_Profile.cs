using AutoMapper;
using Business_AnimeToNotion.Model.Internal;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.Mapper.Internal_Entity
{
    public class Internal_Entity_Profile : Profile
    {
        public Internal_Entity_Profile()
        {
            CreateMap<INT_AnimeShowBase, AnimeShow>()
                .ForMember(dto => dto.Id, map => map.MapFrom(source => Guid.NewGuid()))
                .ForMember(dto => dto.Status, map => map.MapFrom(source => "To Watch"))
                .ForMember(dto => dto.Score, map => map.MapFrom(source => source.Score != null ? new Score()
                {
                    Id = Guid.NewGuid(),
                    MalScore = source.Score.Value
                } : null));                

            CreateMap<INT_AnimeShowFull, AnimeShow>()
                .ForMember(dto => dto.Id, map => map.MapFrom(source => Guid.NewGuid()))
                .ForMember(dto => dto.Status, map => map.MapFrom(source => source.Edit != null && source.Edit.Status != null ? source.Edit.Status : "To Watch"))
                .ForMember(dto => dto.Score, map => map.MapFrom(source => source.Score != null ? new Score()
                {
                    Id = Guid.NewGuid(),
                    MalScore = source.Score.Value,
                    PersonalScore = source.Edit != null && source.Edit.PersonalScore != null ? source.Edit.PersonalScore : null
                } : null))
                .ForMember(dto => dto.WatchingTime, map => map.MapFrom(source => source.Edit != null && source.Edit.StartedOn != null ? new WatchingTime()
                {
                    Id = Guid.NewGuid(),
                    StartedOn = source.Edit.StartedOn.Value,
                    FinishedOn = source.Edit.FinishedOn ?? null,
                    CompletedYear = source.Edit.CompletedYear ?? null,
                } : null))
                .ForMember(dto => dto.Note, map => map.MapFrom(source => source.Edit != null && source.Edit.Notes != null ? new Note()
                {
                    Id = Guid.NewGuid(),
                    Notes = source.Edit.Notes
                } : null))
                .ForMember(dto => dto.Relations, map => map.MapFrom(source => source.Relations.Select(x => new Relation()
                {
                    Id = Guid.NewGuid(),
                    AnimeRelatedMalId = x.RelatedMalId,
                    RelationType = x.Type,
                    Cover = x.Cover
                })));

            CreateMap<INT_KeyValue, Studio>()
                .ForMember(dto => dto.Id, map => map.MapFrom(source => Guid.NewGuid()))
                .ForMember(dto => dto.MalId, map => map.MapFrom(source => source.Id))
                .ForMember(dto => dto.Description, map => map.MapFrom(source => source.Value));

            CreateMap<INT_KeyValue, Genre>()
                .ForMember(dto => dto.Id, map => map.MapFrom(source => Guid.NewGuid()))
                .ForMember(dto => dto.MalId, map => map.MapFrom(source => source.Id))
                .ForMember(dto => dto.Description, map => map.MapFrom(source => source.Value));

            CreateMap<INT_AnimeShowRelation, Relation>()
                .ForMember(dto => dto.Id, map => map.MapFrom(source => Guid.NewGuid()))
                .ForMember(dto => dto.AnimeRelatedMalId, map => map.MapFrom(source => source.RelatedMalId))
                .ForMember(dto => dto.RelationType, map => map.MapFrom(source => source.Type))
                .ForMember(dto => dto.Cover, map => map.MapFrom(source => source.Cover));

        }
    }
}

﻿using AutoMapper;
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
                .ForMember(dto => dto.AnimeShowProgress, map => map.MapFrom(source => new AnimeShowProgress()
                {
                    Id = Guid.NewGuid()
                }));

            CreateMap<INT_AnimeShowFull, AnimeShow>()
                .ForMember(dto => dto.Id, map => map.MapFrom(source => Guid.NewGuid()))
                .ForMember(dto => dto.Status, map => map.MapFrom(source => source.Edit != null && source.Edit.Status != null ? source.Edit.Status : "To Watch"))
                .ForMember(dto => dto.AnimeShowProgress, map => map.MapFrom(source => source.Edit == null ? new AnimeShowProgress()
                {
                    Id = Guid.NewGuid()
                } : new AnimeShowProgress()
                {
                    Id = Guid.NewGuid(),
                    StartedOn = source.Edit.StartedOn,
                    FinishedOn = source.Edit.FinishedOn,
                    CompletedYear = source.Edit.CompletedYear,
                    PersonalScore = source.Edit.PersonalScore,
                    Notes = source.Edit.Notes
                }));

            CreateMap<INT_KeyValue, Studio>()
                .ForMember(dto => dto.Description, map => map.MapFrom(source => source.Value));

            CreateMap<INT_KeyValue, Genre>()
                .ForMember(dto => dto.Description, map => map.MapFrom(source => source.Value));

        }
    }
}

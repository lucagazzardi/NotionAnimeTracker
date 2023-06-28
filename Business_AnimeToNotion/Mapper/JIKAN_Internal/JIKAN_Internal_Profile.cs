using AutoMapper;
using Business_AnimeToNotion.Model.Entities;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.MAL;
using JikanDotNet;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Business_AnimeToNotion.Mapper.FromMal
{
    public class JIKAN_Internal_Profile : Profile
    {
        public JIKAN_Internal_Profile()
        {
            CreateMap<Anime, INT_AnimeShowPartial>()
                .ForMember(dto => dto.Title, map => map.MapFrom(source => 
                    source.Titles.SingleOrDefault(x => x.Type == "English") != null ?
                    source.Titles.Single(x => x.Type == "English").Title : 
                    source.Titles.Single(x => x.Type == "Default").Title))
                .ForMember(dto => dto.MalId, map => map.MapFrom(source => (int)source.MalId))
                .ForMember(dto => dto.Cover, map => map.MapFrom(source => source.Images.JPG.LargeImageUrl));

            CreateMap<Anime, INT_AnimeShowBase>()
                .ForMember(dto => dto.NameEnglish, map => map.MapFrom(source =>
                    source.Titles.SingleOrDefault(x => x.Type == "English") != null ?
                    source.Titles.Single(x => x.Type == "English").Title :
                    source.Titles.Single(x => x.Type == "Default").Title))
                .ForMember(dto => dto.NameDefault, map => map.MapFrom(source => source.Titles.Single(x => x.Type == "Default").Title))
                .ForMember(dto => dto.NameJapanese, map => map.MapFrom(source => source.Titles.Single(x => x.Type == "Japanese").Title))
                .ForMember(dto => dto.MalId, map => map.MapFrom(source => (int)source.MalId!))
                .ForMember(dto => dto.Cover, map => map.MapFrom(source => source.Images.JPG.LargeImageUrl))
                .ForMember(dto => dto.Score, map => map.MapFrom(source => source.Score != null ? Convert.ToInt32(Math.Round(source.Score.Value * 10)) : (int?)null))
                .ForMember(dto => dto.StartedAiring, map => map.MapFrom(source => source.Aired.From))
                .ForMember(dto => dto.Format, map => map.MapFrom(source => source.Type))
                .ForMember(dto => dto.Episodes, map => map.MapFrom(source => source.Episodes))
                .ForMember(dto => dto.Studios, map => map.MapFrom(source => source.Studios.Select(x => new INT_KeyValue((int)x.MalId, x.Name))))
                .ForMember(dto => dto.Genres, map => map.MapFrom(source => source.Genres.Select(x => new INT_KeyValue((int)x.MalId, x.Name))));

            CreateMap<Anime, INT_AnimeShowFull>()
                .ForMember(dto => dto.NameEnglish, map => map.MapFrom(source =>
                    source.Titles.SingleOrDefault(x => x.Type == "English") != null ?
                    source.Titles.Single(x => x.Type == "English").Title :
                    source.Titles.Single(x => x.Type == "Default").Title))
                .ForMember(dto => dto.NameDefault, map => map.MapFrom(source => source.Titles.Single(x => x.Type == "Default").Title))
                .ForMember(dto => dto.NameJapanese, map => map.MapFrom(source => source.Titles.Single(x => x.Type == "Japanese").Title))
                .ForMember(dto => dto.MalId, map => map.MapFrom(source => (int)source.MalId!))
                .ForMember(dto => dto.Cover, map => map.MapFrom(source => source.Images.JPG.LargeImageUrl))
                .ForMember(dto => dto.Score, map => map.MapFrom(source => source.Score != null ? Convert.ToInt32(Math.Round(source.Score.Value * 10)) : (int?)null))
                .ForMember(dto => dto.StartedAiring, map => map.MapFrom(source => source.Aired.From))
                .ForMember(dto => dto.Format, map => map.MapFrom(source => source.Type))
                .ForMember(dto => dto.Episodes, map => map.MapFrom(source => source.Episodes))
                .ForMember(dto => dto.Studios, map => map.MapFrom(source => source.Studios.Select(x => new INT_KeyValue((int)x.MalId, x.Name))))
                .ForMember(dto => dto.Genres, map => map.MapFrom(source => source.Genres.Select(x => new INT_KeyValue((int)x.MalId, x.Name))));


            //CreateMap<List<RelatedEntry>, List<INT_AnimeShowRelation>>().ConvertUsing((src, dest) =>
            //{
            //    List<INT_AnimeShowRelation> result = new List<INT_AnimeShowRelation>();
            //    foreach (var relation in src)
            //    {
            //        foreach (var item in relation.Entry)
            //        {
            //            result.Add(new INT_AnimeShowRelation() { Type = relation.Relation, RelatedMalId = (int)item.MalId, Cover = item.c });
            //        }
            //    }

            //    return result;
            //});
            //

            CreateMap<MAL_RelatedShow, INT_AnimeShowRelation>()
                .ForMember(dto => dto.Type, map => map.MapFrom(source => source.relation_type_formatted))
                .ForMember(dto => dto.RelatedMalId, map => map.MapFrom(source => source.node.id))
                .ForMember(dto => dto.Cover, map => map.MapFrom(source => source.node.main_picture.large));
        }
    }
}

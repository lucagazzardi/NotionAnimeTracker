using AutoMapper;
using Business_AnimeToNotion.Functions.Static;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.MAL;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Model;

namespace Business_AnimeToNotion.Mapper
{
    public class MAL_AnimeShow_AnimeShowDto_Profile : Profile
    {
        public MAL_AnimeShow_AnimeShowDto_Profile()
        {
            // Map from MAL to Entity
            CreateMap<MAL_AnimeShowRaw, AnimeShowDto>()
                .ForMember(dto => dto.MalId, map => map.MapFrom(source => source.id))
                .ForMember(dto => dto.NameOriginal, map => map.MapFrom(source => source.title))
                .ForMember(dto => dto.Cover, map => map.MapFrom(source => source.main_picture.large))
                .ForMember(dto => dto.NameEnglish, map => map.MapFrom(source => string.IsNullOrEmpty(source.alternative_titles.en) ? source.title : source.alternative_titles.en))
                .ForMember(dto => dto.Score, map => map.MapFrom(source => Mapping.Mapper.Map<Score>(source.mean)))
                .ForMember(dto => dto.StartedAiring, map => map.MapFrom(source => DateTime.Parse(source.start_date)))
                .ForMember(dto => dto.Format, map => map.MapFrom(source => Common_Utilities.MALFormatToCommon(source.media_type)))
                .ForMember(dto => dto.Episodes, map => map.MapFrom(source => source.num_episodes))
                .ForMember(dto => dto.Genres, map => map.MapFrom(source => Mapping.Mapper.Map<Dictionary<int, string>>(source.genres)))
                .ForMember(dto => dto.Studios, map => map.MapFrom(source => Mapping.Mapper.Map<Dictionary<int, string>>(source.studios)))
                .ForMember(dto => dto.Relations, map => map.MapFrom(source => Mapping.Mapper.Map<List<RelationDto>>(source.related_anime)));

            // Map from decimal to int MAL Score
            CreateMap<decimal, Score>().ConvertUsing((src, dest) =>
            {
                return new Score()
                {
                    MalScore = Convert.ToInt32(Math.Round(src * 10))
                };
            });

            // Studios and Genres mapping
            CreateMap<List<MAL_GeneralObject>, Dictionary<int, string>>().ConvertUsing((src, dest) =>
            {
                dest = new Dictionary<int, string>();
                foreach (var item in src)
                {
                    dest.Add(item.id, item.name);
                }

                return dest;
            });

            CreateMap<List<MAL_RelatedShow>, List<RelationDto>>().ConvertUsing((src, dest) =>
            {
                dest = new List<RelationDto>();

                foreach (var relation in src)
                {
                    dest.Add(new RelationDto()
                    {
                        RelationType = relation.relation_type,
                        AnimeRelatedMalId = relation.node.id
                    });
                }

                return dest;
            });

        }
    }
}

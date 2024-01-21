using AutoMapper;
using Business_AnimeToNotion.Functions.Static;
using Business_AnimeToNotion.Model.MAL;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.Mapper.NotionSync_MAL
{
    public class NotionSync_MAL_Profile : Profile
    {
        public NotionSync_MAL_Profile()
        {
            CreateMap<AnimeShow, MAL_AnimeUpdateStatus>()
                .ForMember(dto => dto.anime_id, map => map.MapFrom(source => source.MalId))
                .ForMember(dto => dto.status, map => map.MapFrom(source => Common_Utilities.InternalStatusToMAlLUpdateStatus(source.Status)))
                .ForMember(dto => dto.score, map => map.MapFrom(source => Common_Utilities.InternalScoreToMALUpdateScore(source.AnimeShowProgress.PersonalScore)))
                .ForMember(dto => dto.start_date, map => map.MapFrom(source => Common_Utilities.InternalDateToMALDate(source.AnimeShowProgress.StartedOn)))
                .ForMember(dto => dto.end_date, map => map.MapFrom(source => Common_Utilities.InternalDateToMALDate(source.AnimeShowProgress.FinishedOn)))
                .ForMember(dto => dto.updated_at, map => map.MapFrom(source => DateTime.Now.ToString("yyyy-MM-dd")));

        }
    }
}

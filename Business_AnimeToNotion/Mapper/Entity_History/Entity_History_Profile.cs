using AutoMapper;
using Business_AnimeToNotion.Model.History;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.Mapper.Entity_History
{
    public class Entity_History_Profile : Profile
    {
        public Entity_History_Profile()
        {
            CreateMap<AnimeShow, HistoryYearPreview>();
        }
    }
}

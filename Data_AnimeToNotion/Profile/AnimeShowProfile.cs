using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Model;

namespace Data_AnimeToNotion.Profile
{
    public class AnimeShowProfile : AutoMapper.Profile
    {
        public AnimeShowProfile()
        {
            CreateMap<AnimeShow, AnimeShowDto>().ReverseMap();
        }
    }
}

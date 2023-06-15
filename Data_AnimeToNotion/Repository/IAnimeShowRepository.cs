using Data_AnimeToNotion.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_AnimeToNotion.Repository
{
    public interface IAnimeShowRepository
    {
        List<AnimeShow> GetAll();
        AnimeShow Add(AnimeShow animeShow);
        void Update(AnimeShow animeShow);
        void Remove(AnimeShow animeShow);

        void AddStudios(Dictionary<int, string> animeStudios, AnimeShow animeShow);
        void AddGenres(Dictionary<int, string> animeGenres, AnimeShow animeShow);
        void AddWatchingTime(WatchingTime watchingTime, string notionPageId, AnimeShow animeShow);
        void AddScore(Score score, AnimeShow animeShow);
        void AddNote(Note note, AnimeShow animeShow);
        int GetCompletedYearValue(WatchingTime watchingTime);
        bool IsAlreadyExisting(int malId);

        //List<Relation> UpdateRelationsToFill(int animeRelatedMalId, Guid animeShowParentId);
    }
}

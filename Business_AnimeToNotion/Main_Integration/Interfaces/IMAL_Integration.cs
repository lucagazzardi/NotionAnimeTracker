using Business_AnimeToNotion.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Main_Integration.Interfaces
{
    public interface IMAL_Integration
    {
        public Task<List<MAL_AnimeModel>> SearchMALAnimeAsync(string searchTerm);
    }
}

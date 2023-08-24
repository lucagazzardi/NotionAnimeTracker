using Business_AnimeToNotion.Integrations.MAL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace API_AnimeToNotion.Controllers
{
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
    public class MalController : ControllerBase
    {
        private readonly IMAL_Integration _mal;

        public MalController(IMAL_Integration mal)
        {
            _mal = mal;
        }

        /// <summary>
        /// Retrieves anime of the season
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/season/current")]
        public async Task<IActionResult> GetCurrentSeasonAnimeShow()
        {
            return Ok(await _mal.GetCurrentSeasonAnimeShow());
        }

        /// <summary>
        /// Retrieves anime for the upcoming season
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/season/upcoming")]
        public async Task<IActionResult> GetUpcomingSeasonAnimeShow()
        {
            return Ok(await _mal.GetUpcomingSeasonAnimeShow());
        }

        /// <summary>
        /// Searches animes by name
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("search/anime/{query}")]
        public async Task<IActionResult> SearchAnimeByName(string query)
        {
            return Ok(await _mal.SearchAnimeByName(query));
        }

        /// <summary>
        /// Retrieves anime on specific MalId
        /// </summary>
        /// <param name="malId"></param>
        /// <returns></returns>
        [HttpGet("get/anime/{malId}")]
        public async Task<IActionResult> GetAnimeById(int malId)
        {
            return Ok(await _mal.GetAnimeById(malId));
        }

    }
}

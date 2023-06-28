using Business_AnimeToNotion.Functions.Static;
using Business_AnimeToNotion.Integrations.MAL;
using JikanDotNet;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("get/seasonal/{year}/{season}")]
        public async Task<IActionResult> GetSeasonalAnimeShow(int year, string season)
        {
            return Ok(await _mal.GetSeasonalAnimeShow(year, Common_Utilities.GetValueFromDescription<Season>(season)));
        }

        [HttpGet("search/anime/{query}")]
        public async Task<IActionResult> SearchAnimeByName(string query)
        {
            return Ok(await _mal.SearchAnimeByName(query));
        }

        [HttpGet("get/anime/{malId}")]
        public async Task<IActionResult> SearchAnimeByName(int malId)
        {
            return Ok(await _mal.SearchAnimeById(malId));
        }

    }
}

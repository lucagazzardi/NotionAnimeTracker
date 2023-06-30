using Business_AnimeToNotion.Integrations.MAL;
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

        [HttpGet("get/season/current")]
        public async Task<IActionResult> GetCurrentSeasonAnimeShow()
        {
            return Ok(await _mal.GetCurrentSeasonAnimeShow());
        }

        [HttpGet("get/season/upcoming")]
        public async Task<IActionResult> GetUpcomingSeasonAnimeShow()
        {
            return Ok(await _mal.GetUpcomingSeasonAnimeShow());
        }

        [HttpGet("search/anime/{query}")]
        public async Task<IActionResult> SearchAnimeByName(string query)
        {
            return Ok(await _mal.SearchAnimeByName(query));
        }

        [HttpGet("get/anime/{malId}")]
        public async Task<IActionResult> GetAnimeById(int malId)
        {
            return Ok(await _mal.GetAnimeById(malId));
        }

    }
}

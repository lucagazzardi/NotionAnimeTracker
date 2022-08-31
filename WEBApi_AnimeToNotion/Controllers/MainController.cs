using Business_AnimeToNotion.Main_Integration;
using Business_AnimeToNotion.Main_Integration.Interfaces;
using Business_AnimeToNotion.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBApi_AnimeToNotion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainController : ControllerBase
    {
        private readonly IMAL_Integration _malIntegration;
        private readonly INotion_Integration _notionIntegration;

        public MainController(IMAL_Integration malIntegration, INotion_Integration notionIntegration)
        {
            _malIntegration = malIntegration;
            _notionIntegration = notionIntegration;
        }

        [HttpGet("mal/search")]
        public async Task<IActionResult> MAL_SearchAnime([FromQuery] string searchTerm)
        {
            var foundAnimeList = await _malIntegration.MAL_SearchAnimeAsync(searchTerm);
            return Ok(foundAnimeList);
        }
        
        [HttpPost("notion/add")]
        public async Task<IActionResult> Notion_AddNew(MAL_AnimeModel animeModel)
        {
            if (await _notionIntegration.Notion_CreateNewAnimeEntry(animeModel))
                return Ok();
            else
                return BadRequest();
        }
    }
}

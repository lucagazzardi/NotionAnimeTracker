using Business_AnimeToNotion.Main_Integration;
using Business_AnimeToNotion.Main_Integration.Exceptions;
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

        [HttpGet("mal/search/name")]
        public async Task<IActionResult> MAL_SearchAnime([FromQuery] string searchTerm)
        {
            var foundAnimeList = await _malIntegration.MAL_SearchAnimeByNameAsync(searchTerm);
            return Ok(foundAnimeList);
        }

        [HttpGet("mal/search/{id}")]
        public async Task<IActionResult> MAL_SearchAnimeById(int id)
        {
            var foundAnime = await _malIntegration.MAL_SearchAnimeByIdAsync(id);
            return Ok(foundAnime);
        }

        [HttpPost("notion/add")]
        public async Task<IActionResult> Notion_AddNew(MAL_AnimeModel animeModel)
        {
            await _notionIntegration.Notion_CreateNewEntry(animeModel);
            return Ok();            
        }

        [HttpPost("notion/search/{id}/add")]
        public async Task<IActionResult> Notion_SearchByIdAddNew(int id)
        {
            var foundAnime = await _malIntegration.MAL_SearchAnimeByIdAsync(id);

            try 
            {
                await _notionIntegration.Notion_CreateNewEntry(foundAnime);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}

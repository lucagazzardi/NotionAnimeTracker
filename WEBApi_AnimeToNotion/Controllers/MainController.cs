
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace WEBApi_AnimeToNotion.Controllers
//{
//    [ApiController]
//    [EnableCors("AllowAnyOrigin")]
//    [Route("api/[controller]")]
//    public class MainController : ControllerBase
//    {
//        private readonly IMAL_Integration _malIntegration;
//        private readonly INotion_Integration _notionIntegration;

//        public MainController(IMAL_Integration malIntegration, INotion_Integration notionIntegration)
//        {
//            _malIntegration = malIntegration;
//            _notionIntegration = notionIntegration;
//        }

//        [HttpGet("mal/search/name")]
//        public async Task<IActionResult> MAL_SearchAnime([FromQuery] string searchTerm)
//        {
//            if (searchTerm.Length < 3)
//                return Ok(new List<MAL_AnimeModel>());

//            List<MAL_AnimeModel> foundAnimeList = new List<MAL_AnimeModel>();
//            try
//            {
//                foundAnimeList = await _malIntegration.MAL_SearchAnimeByNameAsync(searchTerm);
//            }
//            catch(Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }

//            return Ok(foundAnimeList);
//        }

//        [HttpGet("mal/search/{id}")]
//        public async Task<IActionResult> MAL_SearchAnimeById(int id)
//        {
//            var foundAnime = await _malIntegration.MAL_SearchAnimeByIdAsync(id);
//            return Ok(foundAnime);
//        }

//        [HttpPost("notion/add")]
//        public async Task<IActionResult> Notion_AddNew(MAL_AnimeModel animeModel)
//        {
//            try
//            {
//                await _notionIntegration.Notion_CreateNewEntry(animeModel);
//            }
//            catch(Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }

//            return Ok();            
//        }

//        [HttpPost("notion/search/{id}/add")]
//        public async Task<IActionResult> Notion_SearchByIdAddNew(int id)
//        {
//            var foundAnime = await _malIntegration.MAL_SearchAnimeByIdAsync(id);

//            try 
//            {
//                await _notionIntegration.Notion_CreateNewEntry(foundAnime);
//                return Ok();
//            }
//            catch(Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }            
//        }

//        [HttpGet("notion/get/latestadded")]
//        public async Task<IActionResult> Notion_GetLatestAdded()
//        {
//            List<Notion_LatestAddedModel> result = new List<Notion_LatestAddedModel>();

//            try
//            {
//                result = await _notionIntegration.Notion_GetLatestAdded();
//            }
//            catch(Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }

//            return Ok(result);
//        }

//        [HttpPost("notion/get/toupdate")]
//        public async Task<IActionResult> Notion_GetShowsToUpdate([FromBody] List<string> propertiesToUpdate )
//        {
//            try
//            {
//                return Ok(await _notionIntegration.Notion_UpdateProperties(propertiesToUpdate));
//            }
//            catch(Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//    }
//}

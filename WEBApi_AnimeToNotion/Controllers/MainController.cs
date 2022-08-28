using Business_AnimeToNotion.Main_Integration.Interfaces;
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

        public MainController(IMAL_Integration malIntegration)
        {
            _malIntegration = malIntegration;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMALAnime([FromQuery] string searchTerm)
        {
            var foundAnimeList = await _malIntegration.SearchMALAnimeAsync(searchTerm);
            return Ok(foundAnimeList);
        }
    }
}

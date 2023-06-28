﻿using Business_AnimeToNotion.Integrations.Internal;
using Business_AnimeToNotion.Model.Internal;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API_AnimeToNotion.Controllers
{
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
    public class InternalController : ControllerBase
    {
        private readonly IInternal_Integration _main;

        public InternalController(IInternal_Integration main)
        {
            _main = main;
        }

        [HttpPost("add/base")]
        public async Task<IActionResult> AddAnimeBase([FromBody] INT_AnimeShowAddBase animeBase)
        {
            await _main.AddNewAnimeBase(animeBase);
            return Ok();
        }

        [HttpPost("add/full")]
        public async Task<IActionResult> AddAnimeFull([FromBody] INT_AnimeShowAddFull animeBase)
        {
            await _main.AddNewAnimeFull(animeBase);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> RemoveAnimeShow(Guid id)
        {
            await _main.RemoveAnime(id);
            return Ok();
        }

        #region Demo

        [HttpPost("add/base/demo")]
        public async Task<IActionResult> GetSeasonalAnimeShowDemo([FromBody] INT_AnimeShowAddBase animeBase)
        {            
            return Ok(await _main.AddNewAnimeBaseDemo(animeBase));
        }

        #endregion

    }
}

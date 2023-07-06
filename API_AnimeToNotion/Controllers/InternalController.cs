using Business_AnimeToNotion.Integrations.Internal;
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
        public async Task<IActionResult> AddAnimeBase([FromBody] INT_AnimeShowBase animeBase)
        {
            return Ok(await _main.AddNewAnimeBase(animeBase));
        }

        [HttpPost("add/full")]
        public async Task<IActionResult> AddAnimeFull([FromBody] INT_AnimeShowFull animeBase)
        {
            return Ok(await _main.AddNewAnimeFull(animeBase));
        }

        [HttpGet("get/full/{malId}")]
        public async Task<IActionResult> GetAnimeFull(int malId)
        {
            var anime = await _main.GetAnimeFull(malId);
            if(anime != null)
                return Ok(anime);

            return NotFound();
        }

        [HttpGet("get/edit/{id}")]
        public async Task<IActionResult> GetAnimeForEdit(Guid id)
        {
            var anime = await _main.GetAnimeForEdit(id);
            if (anime != null)
                return Ok(anime);

            return NotFound();
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditAnime([FromBody] INT_AnimeShowEdit animeEdit)
        {
            await _main.EditAnime(animeEdit);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> RemoveAnimeShow(Guid id)
        {
            await _main.RemoveAnime(id);
            return Ok();
        }

        [HttpGet("get/relations/{malId}")]
        public async Task<IActionResult> GetAnimeRelations(int malId)
        {
            return Ok(await _main.GetAnimeRelations(malId));
        }

        #region Demo

        [HttpPost("add/base/demo")]
        public async Task<IActionResult> GetSeasonalAnimeShowDemo([FromBody] INT_AnimeShowBase animeBase)
        {            
            return Ok(await _main.AddNewAnimeBaseDemo(animeBase));
        }

        #endregion

    }
}

﻿using Business_AnimeToNotion.Integrations.Internal;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.Query;
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

        #region AnimeShow

        /// <summary>
        /// Adds base version of the anime, without relations
        /// </summary>
        /// <param name="animeBase"></param>
        /// <returns></returns>
        [HttpPost("add/base")]
        public async Task<IActionResult> AddAnimeBase([FromBody] AnimeShowBase animeBase)
        {
            return Ok(await _main.AddNewAnimeBase(animeBase));
        }

        /// <summary>
        /// Adds full anime, relations included
        /// </summary>
        /// <param name="animeBase"></param>
        /// <returns></returns>
        [HttpPost("add/full")]
        public async Task<IActionResult> AddAnimeFull([FromBody] AnimeShowFull animeBase)
        {
            return Ok(await _main.AddNewAnimeFull(animeBase));
        }

        /// <summary>
        /// Retrieves an anime with relations
        /// </summary>
        /// <param name="malId"></param>
        /// <returns></returns>
        [HttpGet("get/full/{malId}")]
        public async Task<IActionResult> GetAnimeFull(int malId)
        {
            var anime = await _main.GetAnimeFull(malId);
            if(anime != null)
                return Ok(anime);

            return NotFound();
        }

        /// <summary>
        /// Gets already existing anime by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get/edit/{id}")]
        public async Task<IActionResult> GetAnimeForEdit(int id)
        {
            var anime = await _main.GetAnimeForEdit(id);
            if (anime != null)
                return Ok(anime);

            return NotFound();
        }

        /// <summary>
        /// Edits anime
        /// </summary>
        /// <param name="animeEdit"></param>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<IActionResult> EditAnime([FromBody] AnimeShowEdit animeEdit)
        {
            await _main.EditAnime(animeEdit);
            return Ok();
        }

        /// <summary>
        /// Sets anime as favorite or remove
        /// </summary>
        /// <param name="id"></param>
        /// <param name="favorite"></param>
        /// <returns></returns>
        [HttpPut("set/favorite/{id}/{favorite}")]
        public async Task<IActionResult> SetAnimeFavorite(int id, bool favorite)
        {
            return Ok(await _main.SetAnimeFavorite(id, favorite));
        }

        /// <summary>
        /// Sets anime as planned to watch or remove
        /// </summary>
        /// <param name="id"></param>
        /// <param name="plantowatch"></param>
        /// <returns></returns>
        [HttpPut("set/plantowatch/{id}/{plantowatch}")]
        public async Task<IActionResult> SetPlanToWatch(int id, bool plantowatch)
        {
            return Ok(await _main.SetAnimePlanToWatch(id, plantowatch));
        }

        /// <summary>
        /// Deletes anime
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> RemoveAnimeShow(int id)
        {
            await _main.RemoveAnime(id);
            return Ok();
        }

        /// <summary>
        /// Get available episodes from Jikan
        /// </summary>
        /// <param name="animeShowId"></param>
        /// <param name="malId"></param>
        /// <returns></returns>
        [HttpGet("episodes/{malId}")]
        public async Task<IActionResult> GetAnimeEpisodesRecord(int malId)
        {
            return Ok(await _main.GetAnimeEpisodes(malId));
        }

        #endregion

        #region Library

        /// <summary>
        /// Gets a page of shows based on filters, sort and page number specified
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("get/filtered")]
        public async Task<IActionResult> GetLibraryFiltered([FromBody] QueryIn query)
        {            
            return Ok(await _main.LibraryQuery(query.filters, query.sort, query.page));
        }

        #endregion

        #region History

        /// <summary>
        /// Retrieves watched animes grouped by years
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/history")]
        public async Task<IActionResult> GetHistory()
        {
            return Ok(await _main.GetHistory());
        }

        /// <summary>
        /// Retrieves list of animes watched based on year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("get/history/{year}/{page}")]
        public async Task<IActionResult> GetHistoryYear(int year, int page)
        {
            return Ok(await _main.GetHistoryYear(year, page));
        }

        /// <summary>
        /// Retrieves the count of watched and favorite anime for a year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("get/history/count/{year}")]
        public async Task<IActionResult> GetHistoryCount(int year)
        {
            return Ok(await _main.GetHistoryCount(year));
        }

        #endregion

        #region Forms

        [HttpGet("genres")]
        public async Task<IActionResult> GetGenres()
        {
            return Ok(await _main.GetGenres());
        }

        #endregion
    }
}

using Business_AnimeToNotion.Integrations.Demo;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API_AnimeToNotion.Controllers
{
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly IDemo_Integration _demo;

        public DemoController(IDemo_Integration demo)
        {
            _demo = demo;
        }

        #region DEMO

        /// <summary>
        /// Add shows from Notion to database
        /// </summary>
        /// <param name="cursor"></param>
        /// <returns></returns>
        [HttpGet("db/{cursor}")]
        public async Task<IActionResult> FromNotionToDB(string cursor)
        {
            try
            {
                await _demo.FromNotionToDB(cursor);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }        
        }

        /// <summary>
        /// Creates all missing NotionSyncs
        /// </summary>
        /// <returns></returns>
        [HttpGet("db/create/sync")]
        public async Task<IActionResult> CreateNotionSync()
        {
            try
            {
                await _demo.CreateNotionSync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}

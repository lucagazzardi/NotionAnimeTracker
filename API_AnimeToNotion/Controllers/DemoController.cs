using Business_AnimeToNotion.Integrations.Demo;
using Business_AnimeToNotion.Integrations.Notion;
using Business_AnimeToNotion.Model.Notion.Base;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace API_AnimeToNotion.Controllers
{
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly IDemo_Integration _demo;
        private readonly INotion_Integration _notion;

        public DemoController(IDemo_Integration demo, INotion_Integration notion)
        {
            _demo = demo;
            _notion = notion;
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
        /// Demo temp
        /// </summary>
        /// <param name="cursor"></param>
        /// <returns></returns>
        [HttpPost("sync")]
        public async Task<IActionResult> SyncToNotion([FromBody] NotionSyncAdd notionSync)
        {
            try
            {
                _notion.SendSyncToNotion(notionSync);
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

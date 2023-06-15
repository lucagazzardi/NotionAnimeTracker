using Business_AnimeToNotion.Notion;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API_AnimeToNotion.Controllers
{
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly INotion_Integration _notion;

        public DemoController(INotion_Integration notion)
        {
            _notion = notion;
        }

        #region DEMO

        [HttpGet("db/{cursor}")]
        public async Task<IActionResult> FromNotionToDB(string cursor)
        {
            //try
            //{
            //    await _notion.FromNotionToDB(cursor);
            //    return Ok();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            return Ok();             
        }

        #endregion
    }
}

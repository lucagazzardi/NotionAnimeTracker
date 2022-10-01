using Business_AnimeToNotion.Main_Integration;
using Business_AnimeToNotion.Main_Integration.Exceptions;
using Business_AnimeToNotion.Main_Integration.Interfaces;
using Business_AnimeToNotion.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace WEBApi_AnimeToNotion.Controllers
{
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly INotion_Integration _notionIntegration;

        public DemoController(IMAL_Integration malIntegration, INotion_Integration notionIntegration, IConfiguration config)
        {            
            _notionIntegration = notionIntegration;
        }

        [HttpGet("notion/get/ratings")]
        public async Task<IActionResult> Notion_GetRatingsToUpdate()
        {
            List<Notion_RatingsUpdate> result = new List<Notion_RatingsUpdate>();
            try
            {
                result = await _notionIntegration.GetRatingsToUpdate();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }
    }
}

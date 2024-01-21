using Business_AnimeToNotion.MAL_Auth;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API_AnimeToNotion.Controllers
{
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMalAuth _malAuth;

        public AuthController(IMalAuth malAuth)
        {
            _malAuth = malAuth;
        }

        [HttpGet("login")]
        public IActionResult GetCurrentSeasonAnimeShow([FromQuery] string from)
        {
            // Fai controllo che non ci sia già il cookie col valore
            var code_verifier = _malAuth.GeneratePKCECodeVerifier();

            var cookieOptions = new CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(5),
                Path = "/",
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Lax 
            };

            Response.Cookies.Append("code_verifier", code_verifier, cookieOptions);

            string url = _malAuth.BuildAuthorisationUrl(code_verifier, from);

            return Redirect(url);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> GetCurrentSeasonAnimeShow([FromQuery] string code, [FromQuery] string state)
        {
            if (!_malAuth.CheckStateParameter(state))
                return BadRequest("State compromised");

            var code_verifier = Request.Cookies["code_verifier"];

            if (string.IsNullOrEmpty(code_verifier))
                return NotFound("Code Verifier not found");

            await _malAuth.GetAccessToken(code, code_verifier);

            return Ok();
        }
    }
}

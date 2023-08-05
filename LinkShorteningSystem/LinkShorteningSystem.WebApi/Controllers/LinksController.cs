using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using System.Security.Policy;

namespace LinkShorteningSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/links")]
    public class LinksController : ControllerBase
    {
        private readonly ILinkService _linkService;

        public LinksController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        [AllowAnonymous]
        [HttpGet("RedirectLink")]
        public async Task<IActionResult> RedirectLink(string shortenedUrl)
        {
            if (string.IsNullOrEmpty(shortenedUrl))
            {
                return BadRequest("Please provide a shortened URL.");
            }

            try
            {
                var originalUrl = await _linkService.GetOriginalLinkAsync(shortenedUrl);
                if (string.IsNullOrEmpty(originalUrl))
                {
                    return NotFound();
                }

                return Ok(originalUrl);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while redirecting.");
            }
        }       

        [AllowAnonymous]
        [HttpPost("ShortenLink")]
        public async Task<ActionResult<string>> ShortenLink([FromBody] LinkRequest request)
        {
            if (string.IsNullOrEmpty(request.OriginalLink))
            {
                return BadRequest("Please provide an original URL.");
            }

            try
            {
                var shortenedUrl = await _linkService.ShortenLinkAsync(request.OriginalLink);
                return new JsonResult(shortenedUrl);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while shortening the URL.");
            }
        }
    }
}
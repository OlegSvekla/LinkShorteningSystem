using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                return Ok(shortenedUrl);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while shortening the URL.");
            }
        }
    }
}

using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkShorteningSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/links")]
    public class LinkApiController : ControllerBase
    {
        private readonly ILinkService _linkService;
        private readonly ILogger<LinkApiController> _logger;

        public LinkApiController(ILinkService linkService, ILogger<LinkApiController> logger)
        {
            _linkService = linkService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("RedirectLink")]
        public async Task<IActionResult> RedirectLink(string shortenedLink)
        {
            if (string.IsNullOrEmpty(shortenedLink))
            {
                return BadRequest("Please provide a shortened URL.");
            }

            try
            {
                var originalLink = await _linkService.GetOriginalLinkAsync(shortenedLink);
                if (string.IsNullOrEmpty(originalLink))
                {
                    return NotFound();
                }

                return Ok(originalLink);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while redirecting.");
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
                var shortenedLink = await _linkService.ShortenLinkAsync(request.BaseLink, request.OriginalLink);
                return new JsonResult(shortenedLink);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while shortening the URL.");
                return StatusCode(500, "An error occurred while shortening the URL.");
            }
        }
    }
}
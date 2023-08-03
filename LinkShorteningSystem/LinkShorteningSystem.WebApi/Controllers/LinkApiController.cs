using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkShorteningSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkApiController : ControllerBase
    {
        private readonly ILinkService _linkService;

        public LinkApiController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        [HttpPost("ShortenLink")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> ShortenLink([FromBody] LinkDto linkDto)
        {
            if (string.IsNullOrEmpty(linkDto?.OriginalUrl))
            {
                return BadRequest("Please provide an original URL.");
            }

            try
            {
                string shortenedUrl = await _linkService.ShortenLinkAsync(linkDto.OriginalUrl);
                return Ok(shortenedUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while shortening the URL.");
            }
        }
    }
}

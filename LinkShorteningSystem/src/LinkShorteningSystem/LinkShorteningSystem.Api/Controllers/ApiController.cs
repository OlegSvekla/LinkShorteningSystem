using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkShorteningSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/links")]
    public class ApiController : ControllerBase
    {
        private readonly ILinkService _linkService;

        public ApiController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        [AllowAnonymous]
        [HttpGet("RedirectLink")]
        public async Task<IActionResult> RedirectLink(string shortenedLink)
        {
            var originalLink = await _linkService.GetOriginalLinkAsync(shortenedLink);

            return originalLink == 
                null ? NotFound("No original link were found by shortend link") : Ok(originalLink);
        }

        [AllowAnonymous]
        [HttpPost("ShortenLink")]
        public async Task<ActionResult<string>> ShortenLink([FromBody] LinkRequest request)
        {
            var shortenedLink = await _linkService.ShortenLinkAsync(request.BaseLink, request.OriginalLink);

            return shortenedLink == 
                null ? NotFound("Unable to find generatedShortenLink ") : new JsonResult(shortenedLink);
      
        }
    }
}
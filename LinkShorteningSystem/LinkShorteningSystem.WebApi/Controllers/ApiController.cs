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
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILinkService linkService, ILogger<ApiController> logger)
        {
            _linkService = linkService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("RedirectLink")]
        public async Task<IActionResult> RedirectLink(string shortenedLink)
        {
            var originalLink = await _linkService.GetOriginalLinkAsync(shortenedLink);

            return Ok(originalLink);
        }

        [AllowAnonymous]
        [HttpPost("ShortenLink")]
        public async Task<ActionResult<string>> ShortenLink([FromBody] LinkRequest request)
        {
            var shortenedLink = await _linkService.ShortenLinkAsync(request.BaseLink, request.OriginalLink);

            return new JsonResult(shortenedLink);           
        }
    }
}
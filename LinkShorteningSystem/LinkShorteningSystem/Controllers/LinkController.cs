using Microsoft.AspNetCore.Mvc;
using LinkShorteningSystem.HttpClients;

namespace LinkShorteningSystem.Controllers
{
    public class LinkController : Controller
    {
        private readonly ILinkShorteningSystemHttpClient _client;
        private readonly ILogger<LinkController> _logger;

        public LinkController(ILinkShorteningSystemHttpClient client, ILogger<LinkController> logger)
        {
            _client = client;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RedirectLink(string shortenedLink)
        {
            if (string.IsNullOrEmpty(shortenedLink))
            {
                return BadRequest("Please provide a shortened URL.");
            }

            try
            {
                var baseUrl = GetBaseLink();
                var originalUrl = await _client.GetAsync(baseUrl, shortenedLink);

                if (!string.IsNullOrEmpty(originalUrl))
                {
                    return Redirect(originalUrl);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while redirecting.");
                return StatusCode(500, "An error occurred while redirecting.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ShortenLink(string originalLink)
        {
            if (string.IsNullOrEmpty(originalLink))
            {
                ModelState.AddModelError("", "Please enter an original URL.");
                return View("Index");
            }

            try
            {
                var baseLink = GetBaseLink();
                var shortenedLink = await _client.CutLinkAsync(baseLink, originalLink);
                if (string.IsNullOrEmpty(shortenedLink))
                {
                    ModelState.AddModelError("", "An error occurred while shortening the URL.");
                }
                else
                {
                    ViewBag.ShortenedLink = shortenedLink;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while shortening the URL.");
                ModelState.AddModelError("", "An error occurred while shortening the URL.");
                return View("Index");
            }

            return View("Redirect");
        }

        private string GetBaseLink()
        {
            var schema = HttpContext.Request.Scheme;
            var host = HttpContext.Request.Host.Value;
            var baseLink = $"{schema}://{host}";
            return baseLink;
        }
    }
}
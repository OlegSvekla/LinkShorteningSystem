using Microsoft.AspNetCore.Mvc;
using LinkShorteningSystem.HttpClients;

namespace LinkShorteningSystem.Controllers
{
    public class LinkController : Controller
    {
        private readonly ILinkShorteningSystemHttpClient _client;
        
        public LinkController(ILinkShorteningSystemHttpClient client)
        {
            _client = client;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RedirectLink(string shortenedUrl)
        {
            if (string.IsNullOrEmpty(shortenedUrl))
            {
                return BadRequest("Please provide a shortened URL.");
            }

            try
            {
                var baseUrl = GetBaseUrl();
                var originalUrl = await _client.GetAsync(baseUrl, shortenedUrl);

                if (!string.IsNullOrEmpty(originalUrl))
                {
                    return Redirect(originalUrl);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while redirecting.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ShortenLink(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                ModelState.AddModelError("", "Please enter an original URL.");
                return View("Index");
            }

            try
            {
                var baseUrl = GetBaseUrl();
                var shortenedLink = await _client.CutLinkAsync(baseUrl, originalUrl);
                if (string.IsNullOrEmpty(shortenedLink))
                {
                    ModelState.AddModelError("", "An error occurred while shortening the URL.");
                }
                else
                {
                    ViewBag.ShortenedUrl = shortenedLink;
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while shortening the URL.");
                return View("Index");
            }

            return View("Redirect");
        }

        private string GetBaseUrl()
        {
            var schema = HttpContext.Request.Scheme;
            var host = HttpContext.Request.Host.Value;
            var baseUrl = $"{schema}://{host}";
            return baseUrl;
        }
    }
}
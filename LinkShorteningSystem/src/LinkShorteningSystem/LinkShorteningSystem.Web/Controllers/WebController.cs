using Microsoft.AspNetCore.Mvc;
using LinkShorteningSystem.HttpClients;

namespace LinkShorteningSystem.Controllers
{
    public class WebController : Controller
    {
        private readonly ILinkShorteningSystemHttpClient _client;
        private readonly ILogger<WebController> _logger;

        public WebController(ILinkShorteningSystemHttpClient client, ILogger<WebController> logger)
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
            var baseLink = GetBaseLink();

            var originalLink = await _client.GetOriginalByShortendLinkAsync(baseLink, shortenedLink); 
            
            return Redirect(originalLink);            
        }   

        [HttpPost]
        public async Task<IActionResult> ShortenLink(string originalLink)
        {
            var baseLink = GetBaseLink();

            var shortenedLink = await _client.CutLinkAsync(baseLink, originalLink);

            ViewBag.ShortenedLink = shortenedLink;

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
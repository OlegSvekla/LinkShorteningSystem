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
                // using var client = _httpClientFactory.CreateClient();
                // client.BaseAddress = new Uri("https://localhost:7151/");
                //
                // var data = new { OriginalUrl = originalUrl };
                // var jsonContent = new StringContent(JsonSerializer.Serialize(data,
                //     new JsonSerializerOptions { PropertyNameCaseInsensitive = true }), Encoding.UTF8, "application/json");
                //
                // var response = await client.PostAsync("api/LinkApi/ShortenLink", jsonContent);

                var shortenedLink = await _client.CutLinkAsync(originalUrl);
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
            }

            return View("Index");
        }
    }
}

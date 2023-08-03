using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace LinkShorteningSystem.Controllers
{
    public class LinkController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LinkController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
                using (var client = _httpClientFactory.CreateClient())
                {
                    // Устанавливаем базовый адрес для HttpClient (адрес Web API проекта)
                    client.BaseAddress = new Uri("https://localhost:7151/");

                    // Создаем объект для отправки данных в формате JSON
                    var data = new { OriginalUrl = originalUrl };
                    var jsonContent = new StringContent(JsonSerializer.Serialize(data,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }), Encoding.UTF8, "application/json");

                    // Отправляем POST-запрос к Web API проекту
                    var response = await client.PostAsync("api/LinkApi/ShortenLink", jsonContent);

                    // Обработка ответа от Web API проекта
                    if (response.IsSuccessStatusCode)
                    {
                        var shortenedUrl = await response.Content.ReadAsStringAsync();
                        ViewBag.ShortenedUrl = shortenedUrl;
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while shortening the URL.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while shortening the URL.");
            }

            return View("Index");
        }
    }
}

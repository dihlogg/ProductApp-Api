using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WavesOfFoodDemo.Server.Dtos.ChatBot;

namespace WavesOfFoodDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ChatController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var payload = new
            {
                message = request.message,
                userId = request.userId,
            };

            var n8nUrl = "http://localhost:5678/webhook/chatbot";

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(n8nUrl, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                return Content(responseBody, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to connect to n8n", details = ex.Message });
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using WavesOfFoodDemo.Server.Dtos.ChatBot;
using WavesOfFoodDemo.Server.Services;

namespace WavesOfFoodDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConversationService _conversationService;
        public ChatController(IHttpClientFactory httpClientFactory, IConversationService conversationService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _conversationService = conversationService;
        }

        [HttpPost("PostMessage")]
        public async Task<IActionResult> PostMessage([FromBody] ChatRequestDto request)
        {
            try
            {
                var responseBody = await _conversationService.SendMessageAsync(request);
                return Content(responseBody, "text/plain");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetMessages/{userId}")]
        public async Task<IActionResult> GetMessages(Guid userId)
        {
            try
            {
                var messages = await _conversationService.GetMessagesByUserIdAsync(userId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve messages", details = ex.Message });
            }
        }
    }
}

using System.Text;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WavesOfFoodDemo.Server.Dtos.ChatBot;
using WavesOfFoodDemo.Server.Entities;
using WavesOfFoodDemo.Server.Infrastructures.Repositories;

namespace WavesOfFoodDemo.Server.Services.Implements
{
    public class ConversationService : IConversationService
    {
        private readonly ILogger<ConversationService> _logger;
        private readonly IConversationRepository _conversationRepository;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        public ConversationService(IConversationRepository conversationRepository, ILogger<ConversationService> logger, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _conversationRepository = conversationRepository;
            _logger = logger;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Conversations>> GetMessagesByUserIdAsync(Guid userId)
        {
            return await _conversationRepository.GetMessagesByUserIdAsync(userId);
        }

        public async Task<string> SendMessageAsync(ChatRequestDto request)
        {
            var userId = request.userId;

            var userMessage = new Conversations
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Message = request.message,
                Sender = "user",
                Timestamp = DateTime.UtcNow,
                CreateDate = DateTime.UtcNow
            };
            await _conversationRepository.AddMessageAsync(userMessage);

            // send message to webhook n8n
            var payload = new
            {
                message = request.message,
                userId = request.userId
            };
            var n8nUrl = "http://192.168.1.15:5678/webhook/chatbot";
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsync(n8nUrl, content);
            response.EnsureSuccessStatusCode();

            // Parse response fr n8n
            var responseJson = await response.Content.ReadAsStringAsync();
            string responseBody;
            try
            {
                var responseArray = JArray.Parse(responseJson);
                responseBody = responseArray[0]?["output"]?.ToString() ?? "Không có phản hồi từ bot.";
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing n8n response: {ex.Message}");
                responseBody = responseJson; // Fallback: sử dụng response gốc nếu không parse được
            }

            // save bot response
            var botMessage = new Conversations
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Message = responseBody, // Lưu chuỗi HTML
                Sender = "bot",
                Timestamp = DateTime.UtcNow,
                CreateDate = DateTime.UtcNow
            };
            await _conversationRepository.AddMessageAsync(botMessage);

            return responseBody;
        }
    }
}

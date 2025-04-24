using WavesOfFoodDemo.Server.Dtos.ChatBot;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Services
{
    public interface IConversationService
    {
        Task<string> SendMessageAsync(ChatRequestDto request);
        Task<List<Conversations>> GetMessagesByUserIdAsync(Guid userId);
    }
}

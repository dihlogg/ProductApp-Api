using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures.Repositories
{
    public interface IConversationRepository
    {
        Task AddMessageAsync(Conversations message);
        Task<List<Conversations>> GetMessagesByUserIdAsync(Guid userId);
    }
}

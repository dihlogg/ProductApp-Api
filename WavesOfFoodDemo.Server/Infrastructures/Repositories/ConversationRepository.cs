using Microsoft.EntityFrameworkCore;
using WavesOfFoodDemo.Server.DataContext;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures.Repositories
{
    public class ConversationRepository : GenericRepository<Conversations>, IConversationRepository
    {
        public ConversationRepository(ProductDbContext productDbContext) : base(productDbContext)
        {
        }
        public async Task AddMessageAsync(Conversations message)
        {
            _productDbContext.Conversations.Add(message);
            await _productDbContext.SaveChangesAsync();
        }

        public async Task<List<Conversations>> GetMessagesByUserIdAsync(Guid userId)
        {
            return await _productDbContext.Conversations
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Timestamp)
            .ToListAsync();
        }
    }
}

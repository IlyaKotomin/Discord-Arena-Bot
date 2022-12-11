using DiscordArenaBot.Arena.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordArenaBot.Data.Services
{
    internal class PlayerService : IPlayerService
    {
        private readonly BotDbContext _context;

        public PlayerService(BotDbContext context) => _context = context;

        public async Task<Player> GetPlayerByIdAsync(ulong id)
        {
            var player = await _context.Players.Include(n => n.Matches).FirstOrDefaultAsync();

            return player!;
        }

        public async Task RemovePlayerAsync(Player player)
        {
            _context.Remove(player);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            _context.Update(player);

            await _context.SaveChangesAsync();
        }

        public async Task AddPlayerAsync(Player player)
        {
            await _context.Players.AddAsync(player);

            await _context.SaveChangesAsync();
        }
    }
}

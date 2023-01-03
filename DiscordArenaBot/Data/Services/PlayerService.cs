using DiscordArenaBot.Arena.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordArenaBot.Data.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly BotDbContext _context;

        public PlayerService(BotDbContext context) => _context = context;

        public async Task<Player> GetPlayerByIdAsync(ulong id)
        {
            var player = await _context.Players.Where(n => n.DiscordId == id).FirstOrDefaultAsync();

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

        public async Task<List<Player>> GetTopPlayers()
            => await _context.Players.OrderBy(n => -n.Elo).ToListAsync();
    }
}

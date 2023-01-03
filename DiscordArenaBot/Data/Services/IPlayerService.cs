using DiscordArenaBot.Arena.Models;

namespace DiscordArenaBot.Data.Services
{
    public interface IPlayerService
    {
        public Task UpdatePlayerAsync(Player player);

        public Task AddPlayerAsync(Player player);

        public Task RemovePlayerAsync(Player player);

        public Task<Player> GetPlayerByIdAsync(ulong id);

        public Task<List<Player>> GetTopPlayers();
    }
}
    
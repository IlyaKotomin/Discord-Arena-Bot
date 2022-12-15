using DiscordArenaBot.Arena.Models;

namespace DiscordArenaBot.Data.Services
{
    public interface IMatchService
    {
        public Task<List<Match>> GetWins(Player player);

        public Task<List<Match>> GetLoses(Player player);

        public Task UpdateMatch(Match match);

        public Task RemoveMatch(Match match);

        public Task AddMatch(Match match);

        public Task<List<Match>> GetPlayerMatches(Player player, int count);

        public Task<List<Match>> GetPlayerMatches(Player player);
    }
}

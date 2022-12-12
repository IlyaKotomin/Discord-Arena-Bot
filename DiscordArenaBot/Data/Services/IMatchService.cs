using DiscordArenaBot.Arena.Models;

namespace DiscordArenaBot.Data.Services
{
    public interface IMatchService
    {
        public Task UpdateMatch(Match match);

        public Task RemoveMatch(Match match);

        public Task AddMatch(Match match);

        public Task<List<Match>> GetPlayerMatches(Player player, int count);
    }
}

using DiscordArenaBot.Arena.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordArenaBot.Data.Services
{
    public class MatchService : IMatchService
    {
        private readonly BotDbContext _context;

        public MatchService(BotDbContext context) => _context = context;

        public async Task AddMatch(Match match)
        {
            await _context.AddAsync(match);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Match>> GetPlayerMatches(Player player, int count) =>
            await _context.Matches.Where(
                n => player.Id == n.Winner!.Id || player.Id == n.Loser!.Id
                ).Take(count).ToListAsync();

        public async Task<List<Match>> GetPlayerMatches(Player player) =>
            await _context.Matches.Where(
                n => player.Id == n.Winner!.Id || player.Id == n.Loser!.Id
                ).ToListAsync();

        public async Task RemoveMatch(Match match)
        {
            _context.Matches.Remove(match);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateMatch(Match match)
        {
            _context.Matches.Update(match);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Match>> GetWins(Player player)
        {
            return await _context.Matches.Where(n => n.Winner!.DiscordId == player.DiscordId).ToListAsync();
        }

        public async Task<List<Match>> GetLoses(Player player)
        {
            return await _context.Matches.Where(n => n.Loser!.DiscordId == player.DiscordId).ToListAsync();
        }
    }
}

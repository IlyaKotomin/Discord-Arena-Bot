using DiscordArenaBot.Arena.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordArenaBot.Data
{
    internal class BotDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
    }
}

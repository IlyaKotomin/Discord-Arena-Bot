using DiscordArenaBot.Arena.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DiscordArenaBot.Data
{
    public class BotDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(BotSettings.Config["ConnectionString"]);
        }

        public DbSet<Player> Players { get; set; }

        public DbSet<Match> Matches { get; set; }
    }
}

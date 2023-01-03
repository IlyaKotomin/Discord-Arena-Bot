using DiscordArenaBot.Arena.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DiscordArenaBot.Data
{
    public class BotDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(BotSettings.Config["ConnectionString"]);
        }
    }
}

using Discord.Interactions;
using DiscordArenaBot.Data.Contexts;

namespace DiscordArenaBot.Bot.Modules
{
    public class PongModule : InteractionModuleBase<BotSocketInteractionContext>
    {
        [SlashCommand("pong", "Bot test module!")]
        public async Task Pong()
        {
            await RespondAsync("Pong!");
        }
    }
}
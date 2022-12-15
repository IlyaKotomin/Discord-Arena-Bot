using Discord;
using Discord.Interactions;
using DiscordArenaBot.Bot.Modules.ModulesExceptions;
using DiscordArenaBot.Data.Contexts;
using DiscordArenaBot.Arena.Models;
using DiscordArenaBot.Bot;

namespace BroArenaBot
{
    public class RegModule : InteractionModuleBase<BotSocketInteractionContext>
    {
        [SlashCommand("reg", "Register command")]
        public async Task MyModule()
        {
            if (await BotModuleExceptions.UserRegisteredState(Context))
               return;

            var user = Context.User;

            await Context.PlayerService.AddPlayerAsync(new Player(user));

            await RespondAsync(embed: BotEmbeds.Register(user, Context.Guild.IconUrl));
        }
    }
}
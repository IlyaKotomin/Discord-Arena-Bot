using Discord;
using Discord.Interactions;
using DiscordArenaBot.Bot;
using DiscordArenaBot.Bot.Modules.ModulesExceptions;
using DiscordArenaBot.Data.Contexts;

namespace BroArenaBot
{
    public class StatsModule : InteractionModuleBase<BotSocketInteractionContext>
    {
        [SlashCommand("stats", "Shows user stats")]
        public async Task Stats([Summary(name: "User")] IUser? user = null)
        {
            user ??= Context.User;

            if (await BotModuleExceptions.UserUnRegisteredState(user, Context))
                return;

            await RespondAsync(embed: await BotEmbeds.StatsBuilder(user));
        }
    }
}
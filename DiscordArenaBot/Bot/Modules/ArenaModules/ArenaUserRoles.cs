using Discord.Interactions;
using DiscordArenaBot.Arena;
using DiscordArenaBot.Bot.Modules.ModulesExceptions;
using DiscordArenaBot.Data.Contexts;

namespace DiscordArenaBot.Bot.Modules.ArenaModules
{
    public partial class ArenaAdminModules : InteractionModuleBase<BotSocketInteractionContext>
    {
        [SlashCommand("join", "Join to arena")]
        public async Task JoinArenaModule()
        {
            if (await BotModuleExceptions.IsNotArenaStarted(Context.User, Context))
                return;

            if (await BotModuleExceptions.AvailableJoinToArena(Context))
                return;

            if (await BotModuleExceptions.UserUnRegisteredState(Context.User, Context))
                return;

            var player = await Context.PlayerService.GetPlayerByIdAsync(Context.User.Id);

            Matchmaking.PlayersInLine.Add(player);

            await RespondAsync(embed: BotEmbeds.JoinedToArena(Context.User));
        }

        [SlashCommand("left", "Left from arena")]
        public async Task LeftArenaModule()
        {
            if (await BotModuleExceptions.IsNotArenaStarted(Context.User, Context)
                || await BotModuleExceptions.UserUnRegisteredState(Context.User, Context))
                return;

            var player = await Context.PlayerService.GetPlayerByIdAsync(Context.User.Id);

            try
            {
                Matchmaking.PlayersInLine.Remove(player);
            }
            catch (Exception) { }

            await RespondAsync(embed: BotEmbeds.LeftFromArena());
        }

        [SlashCommand("state", "arena state")]
        public async Task StateModule()
        {
            await RespondAsync(Matchmaking.Started.ToString());
        }

        [SlashCommand("top", "Arena`s top")]
        public async Task TopModule()
        {
            var players = await Context.PlayerService.GetTopPlayers();

            await RespondAsync(embed: BotEmbeds.Top25PlayersBuilder(players, Context.Guild.IconUrl));
        }
    }
}

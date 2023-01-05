using Discord.Interactions;
using DiscordArenaBot.Arena;
using DiscordArenaBot.Arena.Models;
using DiscordArenaBot.Bot.Modules.ModulesExceptions;
using DiscordArenaBot.Data.Contexts;
using System.Numerics;

namespace DiscordArenaBot.Bot.Modules.ArenaModules
{
    public partial class ArenaAdminModules : InteractionModuleBase<BotSocketInteractionContext>
    {
        [SlashCommand("join", "Join to arena")]
        public async Task JoinArenaModule()
        {
            if (!await BotModuleExceptions.AvailableJoinToArena(Context))
                return;

            if (await BotModuleExceptions.UserUnRegisteredState(Context.User, Context))
                return;



            Matchmaking.PlayersInLine.Add(await Context.PlayerService.GetPlayerByIdAsync(Context.User.Id));

            await RespondAsync(embed: BotEmbeds.JoinedToArena(Context.User));
        }

        [SlashCommand("leave", "Leave from arena")]
        public async Task LeftArenaModule()
        {
            if (await BotModuleExceptions.UserUnRegisteredState(Context.User, Context))
                return;

            Player player = Matchmaking.GetPlayerFromLineById(Context.User.Id)!;

            if (player != null)
            {
                player.LookingForMatch = false;
                //Matchmaking.RemovePlayerFromList(player, Matchmaking.PlayersInLine);
            }

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

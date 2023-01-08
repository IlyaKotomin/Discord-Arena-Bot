using Discord.Interactions;
using DiscordArenaBot.Arena;
using DiscordArenaBot.Arena.Models;
using DiscordArenaBot.Bot.Modules.ModulesExceptions;
using DiscordArenaBot.Data.Contexts;
using DiscordArenaBot.Data.Services;
using System.Numerics;

namespace DiscordArenaBot.Bot.Modules.ArenaModules
{
    public partial class ArenaAdminModules : InteractionModuleBase<BotSocketInteractionContext>
    {
        [SlashCommand("join", "Join to arena")]
        public async Task JoinArenaModule()
        {
            if (await BotModuleExceptions.IsInArenaLine(Context))
                return;

            if (!await BotModuleExceptions.AvailableJoinToArena(Context))
                return;

            if (await BotModuleExceptions.UserUnRegisteredState(Context.User, Context))
                return;

            PlayerService playerService = new(new(new()));

            var player = await playerService.GetPlayerByIdAsync(Context.User.Id);

            ArenaTop.LocalTopUsers.Add(Context.User, (0, player.Elo));

            player.LookingForMatch = true;

            Matchmaking.PlayersInLine.Add(player);
        
            await RespondAsync(embed: BotEmbeds.JoinedToArena(Context.User));
        }

        [SlashCommand("leave", "Leave from arena")]
        public async Task LeftArenaModule()
        {
            if (await BotModuleExceptions.UserUnRegisteredState(Context.User, Context))
                return;

            Player? player = Matchmaking.PlayersInLine.Where(p => p.DiscordId == Context.User.Id).FirstOrDefault();

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
    }
}

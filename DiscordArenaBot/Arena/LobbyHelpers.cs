using Discord;
using DiscordArenaBot.Arena.Models;
using DiscordArenaBot.Bot;
using DiscordArenaBot.Data.Services;

namespace DiscordArenaBot.Arena
{
    public partial class Lobby
    {

        public static async Task<Lobby> BuildNewLobby(Player player1,
                     Player player2,
                     IPlayerService playerService,
                     IMatchService matchService,
                     IDiscordClient client,
                     IGuild guild)
        {

            var channel = await ConfigurateChannel(player1, player2, client, guild);

            return new Lobby(player1,
                             player2,
                             playerService,
                             matchService,
                             client,
                             guild,
                             channel);
        }

        private static async Task<ITextChannel> ConfigurateChannel(Player player1,
                                                     Player player2,
                                                     IDiscordClient client,
                                                     IGuild guild)
        {
            var player1User = await client.GetUserAsync(player1.Id);
            var player2User = await client.GetUserAsync(player2.Id);

            var channel = await guild.CreateTextChannelAsync($"{player1.DiscordId}vs{player2.DiscordId}");

            await channel.SendMessageAsync($"||<@{player1.Id}><@{player2.Id}>||");

            await channel.AddPermissionOverwriteAsync(guild.EveryoneRole, new OverwritePermissions(viewChannel: PermValue.Deny));
            await channel.AddPermissionOverwriteAsync(player1User, new OverwritePermissions(viewChannel: PermValue.Allow));
            await channel.AddPermissionOverwriteAsync(player2User, new OverwritePermissions(viewChannel: PermValue.Allow));

            await channel.AddPermissionOverwriteAsync(guild.Roles.FirstOrDefault(
                x => x.Id == ulong.Parse(BotSettings.Config["ArenaManagerHostRoleId"]!)),
                new OverwritePermissions(viewChannel: PermValue.Allow));

            var gameInfoMessage = await channel.SendMessageAsync(embed: BotEmbeds.GameInfo(player1, player2));

            Emoji emoteWin1 = new Emoji("1️⃣");
            Emoji emoteWin2 = new Emoji("2️⃣");

            await gameInfoMessage.AddReactionsAsync(new List<Emoji>() { emoteWin1, emoteWin2 });

            return channel;
        }
    }
}
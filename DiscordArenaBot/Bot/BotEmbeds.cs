using Discord;
using DiscordArenaBot.Arena;
using DiscordArenaBot.Arena.Models;
using DiscordArenaBot.Data.Contexts;
using DiscordArenaBot.Data.Extantions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordArenaBot.Bot
{
    public static class BotEmbeds
    {
        public static Embed UnRegistered(IUser user)
        {
            var author = new EmbedAuthorBuilder();
            author.IconUrl = user.GetAvatarUrl();
            author.Name = user.Username;

            var builder = new EmbedBuilder();
            builder.Title = "User is NOT registered!";
            builder.Description = "Please, use the `/reg` command!";
            builder.Color = new Color(235, 64, 52);
            builder.Timestamp = DateTime.Now;
            builder.Author = author;
            return builder.Build();
        }
        public static Embed Register(IUser user, string iconUrl)
        {
            var author = new EmbedAuthorBuilder();
            author.IconUrl = user.GetAvatarUrl();
            author.Name = user.Username;

            var builder = new EmbedBuilder();
            builder.Title = "You have been successfully registered!";
            builder.Description = "Use the `/stats` command to view your statistics!";
            builder.Color = new Color(66, 245, 149);
            builder.Timestamp = DateTime.Now;
            builder.ThumbnailUrl = iconUrl;
            builder.Author = author;
            return builder.Build();
        }
        public static Embed AlreadyRegister(IUser user)
        {
            var author = new EmbedAuthorBuilder();
            author.IconUrl = user.GetAvatarUrl();
            author.Name = user.Username;

            var builder = new EmbedBuilder();
            builder.Title = "You are already registered!";
            builder.Description = "Use the `/stats` command to view your statistics!";
            builder.Color = new Color(66, 245, 149);
            builder.Timestamp = DateTime.Now;
            //builder.ThumbnailUrl = iconUrl;
            builder.Author = author;
            return builder.Build();
        }
        public static Embed UpdateRolesBuilder(IUser user)
        {
            var author = new EmbedAuthorBuilder();
            author.IconUrl = user.GetAvatarUrl();
            author.Name = user.Username;

            var builder = new EmbedBuilder();
            builder.Title = "Updating roles (1-10 minutes)...";
            builder.Description = "Please wait... ";
            builder.Color = new Color(235, 204, 52);
            builder.Timestamp = DateTime.Now;
            builder.Author = author;
            return builder.Build();
        }
        public static Embed JoinedToArena(IUser user)
        {
            var author = new EmbedAuthorBuilder();
            author.IconUrl = user.GetAvatarUrl();
            author.Name = user.Username;

            var builder = new EmbedBuilder();
            builder.Title = "You have been successfully joined to arena!";
            builder.Description = "Wait until the bot picks up an opponent for you!!";
            builder.Color = new Color(66, 245, 149);
            builder.Timestamp = DateTime.Now;
            //builder.ThumbnailUrl = iconUrl;
            builder.Author = author;
            return builder.Build();
        }
        public static Embed AlreadyInArena(IUser user)
        {
            var author = new EmbedAuthorBuilder();
            author.IconUrl = user.GetAvatarUrl();
            author.Name = user.Username;

            var builder = new EmbedBuilder();
            builder.Title = "You are already in line in the arena!";
            builder.Description = "Wait until the bot picks up an opponent for you!!";
            builder.Color = new Color(66, 245, 149);
            builder.Timestamp = DateTime.Now;
            //builder.ThumbnailUrl = iconUrl;
            builder.Author = author;
            return builder.Build();
        }
        public static Embed ArenaNotStarted(IUser user)
        {
            var author = new EmbedAuthorBuilder();
            author.IconUrl = user.GetAvatarUrl();
            author.Name = user.Username;

            var builder = new EmbedBuilder();
            builder.Title = "The arena hasn't started yet!";
            builder.Description = "Please, contact the Arena Manager!";
            builder.Color = new Color(235, 64, 52);
            builder.Timestamp = DateTime.Now;
            builder.Author = author;
            return builder.Build();
        }
        public static Embed ArenaStarted()
        {
            var author = new EmbedAuthorBuilder();
            author.Name = "Arena";

            var builder = new EmbedBuilder();
            builder.Title = "The arena has been successfully launched!";
            builder.Description = "Now you can get in line!";
            builder.Color = new Color(66, 245, 149);
            builder.Timestamp = DateTime.Now;
            builder.Author = author;
            return builder.Build();
        }
        public static Embed ArenaAlreadyStarted()
        {
            var author = new EmbedAuthorBuilder();
            author.Name = "Arena";

            var builder = new EmbedBuilder();
            builder.Title = "The arena is already open!";
            builder.Description = "Now you can get in line!";
            builder.Color = new Color(66, 245, 149);
            builder.Timestamp = DateTime.Now;
            builder.Author = author;
            return builder.Build();
        }
        public static Embed ArenaStoped()
        {
            var author = new EmbedAuthorBuilder();
            author.Name = "Arena";

            var builder = new EmbedBuilder();
            builder.Title = "Arena successfully stopped!";
            builder.Description = "See you next week!";
            builder.Color = new Color(235, 204, 52);
            builder.Timestamp = DateTime.Now;
            builder.Author = author;
            return builder.Build();
        }
        public static Embed UnRegisteredInArena(IUser user)
        {
            var author = new EmbedAuthorBuilder();
            author.IconUrl = user.GetAvatarUrl();
            author.Name = user.Username;

            var builder = new EmbedBuilder();
            builder.Title = "You are out of line!";
            builder.Description = "Please, use the `/join` command!";
            builder.Color = new Color(235, 64, 52);
            builder.Timestamp = DateTime.Now;
            builder.Author = author;
            return builder.Build();
        }
        public static Embed LeftFromArena()
        {
            var author = new EmbedAuthorBuilder();
            author.Name = "Arena";

            var builder = new EmbedBuilder();
            builder.Title = "You have successfully exited the arena queue!";
            builder.Description = "See you next week!";
            builder.Color = new Color(235, 204, 52);
            builder.Timestamp = DateTime.Now;
            builder.Author = author;
            return builder.Build();
        }

        public static  async Task<Embed> StatsBuilder(IUser user, BotSocketInteractionContext context)
        {
            Player player = await context.PlayerService.GetPlayerByIdAsync(user.Id);
            var allMatches = await context.MatchService.GetPlayerMatches(player);
            var last5Matches = await context.MatchService.GetPlayerMatches(player, 5);




            var author = new EmbedAuthorBuilder();
            author.IconUrl = user.GetAvatarUrl();
            author.Name = user.Username;

            var fields = new List<EmbedFieldBuilder>();
            fields.Add(new EmbedFieldBuilder() { Name = "⠀⠀⠀Elo", Value = "⠀⠀⠀" + player.Elo, IsInline = true });
            fields.Add(new EmbedFieldBuilder() { Name = "Wins", Value = player.Wins, IsInline = true });
            fields.Add(new EmbedFieldBuilder() { Name = "Loses", Value = player.Loses, IsInline = true });
            fields.Add(new EmbedFieldBuilder() { Name = "⠀⠀⠀Total Games", Value = "⠀⠀⠀" + player.TotalGames, IsInline = true });
            fields.Add(new EmbedFieldBuilder() { Name = "Max Win Streak", Value = player.GetStreak(last5Matches, Data.Enums.MatchStreakType.Wins), IsInline = true });
            fields.Add(new EmbedFieldBuilder() { Name = "Max Lose Streak", Value = player.GetStreak(last5Matches, Data.Enums.MatchStreakType.Loses), IsInline = true });
            fields.Add(new EmbedFieldBuilder() { Name = "╚═════════════════════════════════════╝", Value = "_ _", IsInline = false });
            fields.Add(new EmbedFieldBuilder() { Name = "Last games", Value = player.GetEmojiLastGamesString(last5Matches), IsInline = false });

            var builder = new EmbedBuilder();
            //builder.Title = "Rank: " + GetMedalEmote(player.Level);
            builder.Description = "**╔═════════════════════════════════════╗**";
            //builder.ThumbnailUrl = GetTrophyImgUrl(player.Level);
            //builder.Color = GetColorByLvl(player.Level);
            //builder.ImageUrl = user.GetAvatarUrl();
            builder.Fields = fields;
            builder.Author = author;

            return builder.Build();
        }

        public static Embed GameInfo(Player player1, Player player2)
        {
            var builder = new EmbedBuilder();
            builder.Title = "Arena 1v1";
            //builder.Description = "Сlick on the winner's emoji to end the match!\n||**User Elo: 'elo' (+Elo if win / -Elo if lose)**||";

            var fields = new List<EmbedFieldBuilder>();

            fields.Add(new EmbedFieldBuilder()
            {
                Name = $"1️⃣ Elo: {player1.Elo} (+{EloRatingSystem.CalculateDelta(player1, player2)}/-{EloRatingSystem.CalculateDelta(player2, player1)})",
                Value = $"<@{player1.Id}>",
                IsInline = false
            });

            fields.Add(new EmbedFieldBuilder()
            {
                Name = $"2️⃣ Elo: {player2.Elo} (+{EloRatingSystem.CalculateDelta(player2, player1)}/-{EloRatingSystem.CalculateDelta(player1, player2)})",
                Value = $"<@{player2.Id}>",
                IsInline = false
            });

            builder.Fields = fields;
            builder.Color = new Color(235, 204, 52);

            //var footer = new EmbedFooterBuilder();
            //footer.Text = $"Expectations: {Math.Round(Elo.ExpectationToWin(player1.Elo, player2.Elo), 2)}% / " +
            //$"{Math.Round(Elo.ExpectationToWin(player2.Elo, player1.Elo), 2)}%";
            //builder.Footer = footer;
            builder.Timestamp = DateTime.Now;

            return builder.Build();
        }
        public static Embed EmoteCheckBuilder(Player player)
        {
            var builder = new EmbedBuilder();
            builder.Description = $"Add reaction here if <@{player.Id}> won!";
            return builder.Build();
        }
        public static Embed Top25PlayersBuilder(List<Player> players, string imgUrl)
        {
            string description = "";

            var builder = new EmbedBuilder();
            builder.Title = "FDG Arena top players";
            builder.ThumbnailUrl = imgUrl;
            builder.Color = new Color(52, 235, 89);

            var fields = new List<EmbedFieldBuilder>();

            for (int i = 0; i < players.Count(); i++)
                description += $"{BotSettings.GetMedalEmote(players[i].Level)}**{i + 1}:**<@{players[i].Id}> ** - ** elo: {players[i].Elo}\n";

            builder.Description = description;
            builder.Fields = fields;

            return builder.Build();
        }
    }
}

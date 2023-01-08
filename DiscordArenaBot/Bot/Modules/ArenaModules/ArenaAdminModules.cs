using Discord;
using Discord.Interactions;
using Discord.Rest;
using DiscordArenaBot.Arena;
using DiscordArenaBot.Bot.Modules.ModulesExceptions;
using DiscordArenaBot.Data.Contexts;
using Microsoft.IdentityModel.Tokens;
using System;

namespace DiscordArenaBot.Bot.Modules.ArenaModules
{
    [Group("arena", "Сommands for interacting with the arena")]
    public partial class ArenaAdminModules : InteractionModuleBase<BotSocketInteractionContext>
    {
        private static RestGuildEvent? _event;

        [SlashCommand("start", "Arena launch")]
        [RequireRole("Arena Manager")]
        public async Task StartArenaModule(string? streamUrl = null)
        {
            if (streamUrl != null)
                await Context.Client.SetGameAsync("Watch our stream!", streamUrl, ActivityType.Streaming);

            if (await BotModuleExceptions.IsAlreadyArenaStarted(Context))
                return;

            //if(_event is not null)
            //    await _event.StartAsync();
            Matchmaking.Start(Context.Client, Context.Guild);

            ITextChannel? logChannel = Context.Guild.GetTextChannel(BotSettings.LogChannelId);

            await logChannel.SendMessageAsync("```The Arena was launched!```<@&1061219455025303562>");

            await RespondAsync(embed: BotEmbeds.ArenaStarted());

        }

        [SlashCommand("stop", "Arena launch")]
        [RequireRole("Arena Manager")]
        public async Task StopArenaModule()
        {
            if (await BotModuleExceptions.IsNotArenaStarted(Context.User, Context))
                return;

            Matchmaking.Stop();

            foreach (var @event in await Context.Guild.GetEventsAsync())
                await @event.DeleteAsync();

            ITextChannel? logChannel = Context.Guild.GetTextChannel(BotSettings.LogChannelId);
            ITextChannel? mainChannel = Context.Guild.GetTextChannel(BotSettings.LogChannelId);

            await mainChannel.SendMessageAsync(embed: ArenaTop.GetLocalTopEmbed(Context.Guild.IconUrl));
            await logChannel.SendMessageAsync("```The Arena was stoped!```<@&1061219455025303562>");

            await RespondAsync(embed: BotEmbeds.ArenaStoped());
        }

        [SlashCommand("anonce", "Arena anonce")]
        [RequireRole("Arena Manager")]
        public async Task AnonceArenaModule(int number, int hour, int? day = null, int? month = null)
        {
            month ??= DateTime.Now.Month;
            day ??= DateTime.Now.Day;


            DateTimeOffset startTime= new DateTime(DateTime.Now.Year, (int)month, (int)day, hour, 0, 0);
            DateTimeOffset endTime = startTime.AddHours(2);

            _event = await Context.Guild.CreateEventAsync($"ESG Arena #{number}",
                                                                  startTime: startTime,
                                                                  GuildScheduledEventType.External,
                                                                  endTime: endTime,
                                                                  location: "ESG",
                                                                  description: BotSettings.EventDescription);


            ITextChannel? mainChannel = Context.Guild.GetTextChannel(BotSettings.MainChannelId);

            await mainChannel.SendMessageAsync($"<@&1061219455025303562> \r\n\r\n**ESG Arena #{number}**\r\n\r\n**Start time:** <t:{startTime.ToUnixTimeSeconds()}>\r\n\r\n**firstly check** <#1059248012146257940>\r\n\r\nYou must write `/reg` in <#1046207342808662070> (if you are not registered) and `/arena join` when it will start.");

            await RespondAsync("Anonce created.");
        }

        [SlashCommand("stream", "Sets bot`s stream")]
        [RequireRole("streamer")]
        public async Task SetStream(string url)
        {
            await Context.Client.SetGameAsync("Watch our stream!", url, ActivityType.Streaming);

            await RespondAsync($"Stream activity changed! ({url})");
        }
    }
}
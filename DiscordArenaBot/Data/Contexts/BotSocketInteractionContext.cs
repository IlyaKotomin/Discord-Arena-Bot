using Discord.Interactions;
using Discord.WebSocket;
using DiscordArenaBot.Data.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordArenaBot.Data.Contexts
{
    public class BotSocketInteractionContext : SocketInteractionContext
    {
        public IPlayerService PlayerService { get; private set; }

        public BotSocketInteractionContext(DiscordSocketClient client,
                                           SocketInteraction interaction,
                                           IServiceProvider serviceProvider)
                                            : base(client, interaction)
        {
            PlayerService = serviceProvider.GetRequiredService<IPlayerService>();
        }
    }
}

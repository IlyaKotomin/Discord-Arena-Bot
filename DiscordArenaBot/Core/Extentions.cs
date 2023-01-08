using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordArenaBot.Core
{
    public static class Extentions
    {
        public static int GetCharStreak(this string @string, char @char)
        {
            int currentIndex = 0;
            int maximum = 0;

            for (int i = 0; i < @string.Length; i++)
                if (@string[i] == @char)
                {
                    currentIndex++;
                    if (currentIndex > maximum)
                        maximum = currentIndex;
                }
                else
                    currentIndex = 0;
            return maximum;
        }
    }
}

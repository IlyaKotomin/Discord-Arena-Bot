using DiscordArenaBot.Arena.Models;

namespace DiscordArenaBot.Arena
{
    public class EloRatingSystem
    {
        public static void CalculateRating(Player winner, Player loser)
        {
            double winnerK = GetKFactor(winner);
            double loserK = GetKFactor(loser);

            double resultK = (winnerK + loserK) / 2.0;

            int deltaElo = (int)(resultK * (1 - Getexpectation(winner, loser)));

            winner.Elo += deltaElo;
            loser.Elo -= deltaElo;
        }


        private static double GetKFactor(Player player)
        {
            int elo = player.Elo;

            switch (elo)
            {
                case < 800:
                    return 40;
                case < 950:
                    return 35;
                case < 1250:
                    return 30;
                case < 1400:
                    return 25;
                case < 1550:
                    return 20;
                case < 1700:
                    return 15;
                case < 1850:
                    return 10;
                default:
                    return 5;
            }
        }


        private static double GetQFactor(Player player) =>
            Math.Pow(10.0, (double)player.Elo / 400.0);


        private static double Getexpectation(Player expectationWinner, Player expectationLoser)
        {
            double winnerQFactor = GetQFactor(expectationWinner);
            double loserQFactor = GetQFactor(expectationLoser);

            return winnerQFactor / (winnerQFactor + loserQFactor);
        }
    }
}

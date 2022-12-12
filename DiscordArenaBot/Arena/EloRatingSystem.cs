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


        private static double GetKFactor(Player player) => player.Elo switch
        {
            < 800  => 40,
            < 950  => 35,
            < 1250 => 30,
            < 1400 => 25,
            < 1550 => 20,
            < 1700 => 15,
            < 1850 => 10,
            _ => 5,
        };


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

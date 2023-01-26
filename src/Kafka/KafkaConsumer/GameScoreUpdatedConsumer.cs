using System.Collections.Concurrent;
using Blofeld;
using Chabis.EventStreaming;

namespace KafkaConsumer;

[KafkaConsumer("dg-blofeld-gamescore-v1")]
public class GameScoreUpdatedConsumer : IKafkaMessageConsumer<long, GameScoreUpdated>
{
    private static ConcurrentDictionary<int, (int UserId, int HighScore)> HighScore = new();
    private static ConcurrentBag<int> DeletedGames = new();

    public Task HandleAsync(ConsumerContext<long, GameScoreUpdated> context, CancellationToken cancellationToken)
    {
        try
        {
            /*
             * PLACE YOUR CODE HERE
             */
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }

        return Task.CompletedTask;
    }

    private (int UserId, int HighScore) UpdateHighscore(int gameId, int userId, int highScore, string consumer)
    {
        /*
         * DO NOT EDIT THIS
         */

        if (ShouldFail())
            throw new InvalidOperationException("Artificial network timeout");

        if (DeletedGames.Contains(gameId))
            throw new InvalidOperationException($"Game {gameId} has already been deleted");

        var result = HighScore.AddOrUpdate(
            gameId,
            _ => (UserId: userId, HighScore: highScore),
            (gameId, lastUserHighScore) =>
            {
                if (lastUserHighScore.HighScore >= highScore)
                    throw new InvalidOperationException(
                        $"Highscore for game {gameId} could not decrease from {lastUserHighScore.HighScore} to {lastUserHighScore.HighScore}");

                return (UserId: userId, HighScore: highScore);
            });

        Console.WriteLine($"[{consumer}] New Highscore for game {gameId} user {userId} reached {highScore}");

        return result;
    }

    private void DeleteHighScore(int gameId, string consumer)
    {
        /*
         * DO NOT EDIT THIS
         */

        if (ShouldFail())
            throw new InvalidOperationException("Artificial network timeout");

        if (DeletedGames.Contains(gameId))
            throw new InvalidOperationException($"Game {gameId} has already been deleted");

        HighScore.TryRemove(gameId, out _);
        DeletedGames.Add(gameId);

        Console.WriteLine($"[{consumer}] Game {gameId} removed");
    }

    private bool ShouldFail()
    {
        /*
         * DO NOT EDIT THIS
         */

        return Random.Shared.NextDouble() < 0.01;
    }
}
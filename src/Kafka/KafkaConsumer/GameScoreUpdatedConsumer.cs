using System.Collections.Concurrent;
using Blofeld;
using Chabis.EventStreaming;

namespace KafkaConsumer;

[KafkaConsumer(
    "dg-blofeld-gamescore-v1",
    RetryStrategy = RetryStrategy.RetryWithExponentialBackOff,
    MaxRetryAttempts = 2,
    ErrorAction = ErrorAction.Stop)]
public class GameScoreUpdatedConsumer : IKafkaMessageConsumer<int, GameScoreUpdated>
{
    private static ConcurrentDictionary<int, (int UserId, int HighScore)> HighScore = new();

    public Task HandleAsync(ConsumerContext<int, GameScoreUpdated> context, CancellationToken cancellationToken)
    {
        var gameScore = context.Event.Entity;

        if (HighScore.TryGetValue(gameScore.GameId, out var userHighScore))
        {
            if (userHighScore.HighScore < gameScore.Score)
            {
                UpdateHighscore(gameScore.GameId, gameScore.UserId, gameScore.Score);
                Console.WriteLine($"[{context.ConsumerName}] New Highscore for game {gameScore.GameId} user {gameScore.UserId} reached {gameScore.Score}");
            }
        }

        return Task.CompletedTask;
    }

    private (int UserId, int HighScore) UpdateHighscore(int gameId, int userId, int highScore)
    {
        return HighScore.AddOrUpdate(
            gameId,
            _ => (UserId: userId, HighScore: highScore),
            (gameId, lastUserHighScore) =>
            {
                if (lastUserHighScore.HighScore >= highScore)
                    throw new InvalidOperationException(
                        $"Highscore for game {gameId} could not decrease from {lastUserHighScore.HighScore} to {lastUserHighScore.HighScore}");

                return (UserId: userId, HighScore: highScore);
            });
    }
}
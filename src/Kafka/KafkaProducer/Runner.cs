using Avro.Specific;
using Blofeld;
using Chabis.EventStreaming;

namespace KafkaProducer;

public class Runner
{
    private readonly IKafkaProducer<long, ISpecificRecord> _kafkaProducer;
    private readonly HashSet<int> _deletedGames = new();

    public Runner(IKafkaProducer<long, ISpecificRecord> kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }

    public async Task PublishAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var userId = Random.Shared.Next(1, 1000);
            var gameId = Random.Shared.Next(1, 1000);

            if (_deletedGames.Contains(gameId))
                continue;

            var gameScore = Random.Shared.NextDouble() > 0.01
                ? new GameScore
                    {
                        UserId = userId,
                        GameId = gameId,
                        Score = Random.Shared.Next(0, int.MaxValue)
                    }
                : null;

            var gameScoreUpdated = new GameScoreUpdated
            {
                EntityId = gameId,
                Entity = gameScore,
                EventType = gameScore == null ? DomainEntityEventType.Deleted : DomainEntityEventType.Updated,
                UpdateDate = DateTime.UtcNow
            };

            _kafkaProducer.Produce(
                topic: "dg-blofeld-gamescore-v1",
                key: gameScoreUpdated.EntityId,
                message: gameScoreUpdated);

            if (gameScore == null)
                _deletedGames.Add(gameId);

            await Task.Delay(100);
        }
    }
}
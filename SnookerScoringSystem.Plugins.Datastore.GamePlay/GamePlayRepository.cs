using SnookerScoringSystem.Domain;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.Plugins.Datastore.GamePlay
{
    // All the code in this file is included in all platforms.
    public class GamePlayRepository : IGamePlayRepository
    {
        public Task CalculateScoreAsync(List<DetectedBall> detectedBalls)
        {
            throw new NotImplementedException();
        }
    }
}

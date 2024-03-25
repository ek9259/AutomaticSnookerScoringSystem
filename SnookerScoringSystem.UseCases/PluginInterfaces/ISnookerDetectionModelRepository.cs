using SnookerScoringSystem.Domain;

namespace SnookerScoringSystem.UseCases.PluginInterfaces
{
    public interface ISnookerDetectionModelRepository
    {
        Task<List<DetectedBall>> DetectSnookerBallAsync(string framePath);

    }
}

using SnookerScoringSystem.Domain;

namespace SnookerScoringSystem.UseCases.Interfaces
{
    public interface IDetectSnookerBallUseCase
    {
        Task<List<DetectedBall>> ExecuteAsync(string framePath);
    }
}

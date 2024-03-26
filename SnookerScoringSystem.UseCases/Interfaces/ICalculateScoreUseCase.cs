namespace SnookerScoringSystem.UseCases.Interfaces
{
    public interface ICalculateScoreUseCase
    {
        Task ExecuteAsync(List<Domain.DetectedBall> detectedBalls);
    }
}

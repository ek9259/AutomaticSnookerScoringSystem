using SnookerScoringSystem.Domain;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.UseCases
{
    public class CalculateScoreUseCase : ICalculateScoreUseCase
    {
        private readonly IGamePlayRepository _gamePlayRepository;
        public CalculateScoreUseCase(IGamePlayRepository gamePlayRepository)
        {
            this._gamePlayRepository = gamePlayRepository;
        }
        public async Task ExecuteAsync(List<DetectedBall> detectedBalls)
        {
            await _gamePlayRepository.CalculateScoreAsync(detectedBalls);
        }
    }
}

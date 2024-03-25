using SnookerScoringSystem.Domain;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.UseCases
{
    public class DetectSnookerBallUseCase : IDetectSnookerBallUseCase
    {
        private readonly ISnookerDetectionModelRepository _modelRepository;
        public DetectSnookerBallUseCase(ISnookerDetectionModelRepository modelRepository)
        {
            this._modelRepository = modelRepository;
        }

        public async Task<List<DetectedBall>> ExecuteAsync(string framePath)
        {
            return await _modelRepository.DetectSnookerBallAsync(framePath);
        }
    }
}

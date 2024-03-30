using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.UseCases
{
    public class StopExtractingFrameUseCase : IStopExtractingFrameUseCase
    {
        private readonly IVideoProcessingRepository _videoProcessingRepository;

        public StopExtractingFrameUseCase(IVideoProcessingRepository videoProcessingRepository)
        {
            this._videoProcessingRepository = videoProcessingRepository;
        }
        public void Execute()
        {
            this._videoProcessingRepository.StopExtractingFrame();
        }
    }
}

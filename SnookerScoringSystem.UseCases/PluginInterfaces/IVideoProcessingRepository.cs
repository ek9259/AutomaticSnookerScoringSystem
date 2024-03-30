namespace SnookerScoringSystem.UseCases.PluginInterfaces
{
    public interface IVideoProcessingRepository
    {
        Task ExtractFrameAsync();
        string GetVideoPath();
        void StopExtractingFrame();
    }
}
using SnookerScoringSystem.GameplayServices.PluginInterfaces;

namespace SnookerScoringSystem.GameplayServices
{
    public class FrameWatcherService : IFrameWatcherService
    {
        private FileSystemWatcher _fileSystemWatcher;

        public event Action<string> FrameChanged;

        public FrameWatcherService()
        {
            SetUpFrameWatcher();
        }

        private void SetUpFrameWatcher()
        {
            this._fileSystemWatcher = new FileSystemWatcher();
            this._fileSystemWatcher.Path = Path.Combine(FileSystem.Current.AppDataDirectory);
            this._fileSystemWatcher.Filter = "frame.jpg";
            this._fileSystemWatcher.Changed += OnFrameChanged;
            this._fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void OnFrameChanged(object sender, FileSystemEventArgs e)
        {
            string framePath = e.FullPath;
            FrameChanged?.Invoke(framePath);
        }
    }
}

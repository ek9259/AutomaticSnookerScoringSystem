using Emgu.CV;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.GameplayServices
{
    public class LiveVideoCaptureService : IVideoProcessingRepository
    {
        private VideoCapture _videoCapture;
        
        private CancellationTokenSource _cancellationTokenSource;

        public LiveVideoCaptureService()
        {
            try
            {
                // Access video from camera
                this._videoCapture = new VideoCapture(0);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while accessing the camera: {ex.Message}", ex);
            }
        }
        public async Task ExtractFrameAsync()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
            }

            _cancellationTokenSource = new CancellationTokenSource();

            await Task.Run(async () =>
            {
                if (this._videoCapture.IsOpened)
                {
                    int frameCount = 0;
                    int fps = 30;
                    int second = 1;
                    while (true)
                    {
                        Mat frame = this._videoCapture.QueryFrame();
                        if (_cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            break;
                        }

                        //this._videoCapture.Read(frame);

                        if (frame.IsEmpty)
                        {
                            break;
                        }

                        if (frameCount % fps == 0)
                        {
                            // Get the AppData directory path
                            string appDataDirectory = FileSystem.Current.AppDataDirectory;

                            // Create the full file path
                            string filePath = Path.Combine(appDataDirectory, "frame.jpg");

                            CvInvoke.Imwrite(filePath, frame);
                            await Task.Delay(200);
                        }
                        frameCount++;
                    }
                }
            }); 
        }

        public string GetVideoPath()
        {
            var videoPath = Path.Combine(AppContext.BaseDirectory, "Videos", "Snooker_Video.mp4");

            if (!File.Exists(videoPath))
            {
                throw new FileNotFoundException($"The video file cannot be found at path: {videoPath}");
            }

            return videoPath;
        }
         
        public void StopExtractingFrame()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }
        }
    }
}

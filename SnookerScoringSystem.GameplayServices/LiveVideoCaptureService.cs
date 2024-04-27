using CommunityToolkit.Mvvm.ComponentModel;
using Emgu.CV;
using Emgu.CV.CvEnum;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.GameplayServices
{
    public class LiveVideoCaptureService : IVideoProcessingRepository
    {
        private VideoCapture? _videoCapture;

        //private VideoWriter _writer;
        
        private CancellationTokenSource? _cancellationTokenSource;

        private string _appDataDirectory = FileSystem.Current.AppDataDirectory;

        private string _liveVideoCapturePath;

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
                    Mat frame = new Mat();
                    double frameCount = 0;
                    double fps = this._videoCapture.Get(CapProp.Fps);

                    //How many frame extracted per seconds
                    double frequency = 6;

                    while (true)
                    {
                        if (_cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            break;
                        }

                        this._videoCapture.Read(frame);

                        if (frame.IsEmpty)
                        {
                            break;
                        }

                        if (frameCount % (fps / frequency) == 0)
                        {
                            // Create the full file path
                            string filePath = Path.Combine(_appDataDirectory, "frame.jpg");

                            CvInvoke.Imwrite(filePath, frame);
                        }

                        frameCount++;
                    }
                }
            }); 
        }

        public async Task SetUpVideoCaptureAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (_videoCapture == null)
                    {
                        // Access video from camera
                        this._videoCapture = new VideoCapture(0);
                    }
                    _videoCapture.Start();
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while accessing the camera: {ex.Message}", ex);
                }
            });
        }

        public string GetVideoPath()
        {
            var videoPath = this._liveVideoCapturePath;

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

            if (this._videoCapture != null)
            {
                this._videoCapture.Stop();
            }
            this._videoCapture = null;
        }
    }
}

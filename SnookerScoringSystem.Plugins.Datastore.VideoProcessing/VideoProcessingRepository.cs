using System.Reflection;
using Emgu.CV;
using Emgu.CV.Util;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.Plugins.Datastore.VideoProcessing
{
    // All the code in this file is included in all platforms.
    public class VideoProcessingRepository :IVideoProcessingRepository
    {
        private readonly VideoCapture _snookerVideo;
        private readonly string _videoPath;
        public VideoProcessingRepository()
        {
            try
            {
                // Get the path to the video file
                _videoPath = Path.Combine(AppContext.BaseDirectory, "Videos", "Snooker_Video.mp4");

                if (!File.Exists(_videoPath))
                {
                    throw new Exception($"File not found: {_videoPath}");
                }

                _snookerVideo = new VideoCapture(_videoPath);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while accessing the video", ex);
            }
        }

        public async Task ExtractFrameAsync()
        {
            await Task.Run(async () =>
            {
                if (_snookerVideo.IsOpened)
                {
                    Mat frame = new Mat();
                    int frameCount = 0;
                    int fps = 60;
                    int second = 1;
                    while (true)
                    {
                        _snookerVideo.Read(frame);

                        if (frame.IsEmpty)
                        {
                            break;
                        }

                        if (frameCount % (fps * 1) == 0)
                        {
                            // Get the AppData directory path
                            string appDataDirectory = FileSystem.Current.AppDataDirectory;

                            // Create the full file path
                            string filePath = Path.Combine(appDataDirectory, "frame.jpg");

                            //// If the file exists, delete it
                            //if (File.Exists(filePath))
                            //{
                            //    File.Delete(filePath);
                            //}

                            CvInvoke.Imwrite(filePath, frame);
                            await Task.Delay(900);
                        }
                        frameCount++;
                    }
                }
            });
        }
    }
}

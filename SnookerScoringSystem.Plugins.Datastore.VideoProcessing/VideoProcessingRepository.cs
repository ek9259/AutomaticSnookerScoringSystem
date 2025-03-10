﻿using System.Reflection;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.Plugins.Datastore.VideoProcessing
{
    // All the code in this file is included in all platforms.
    public class VideoProcessingRepository :IVideoProcessingRepository
    {
        private VideoCapture _snookerVideo;
        private string _videoPath;

        private CancellationTokenSource _cancellationTokenSource;


        //Find the path to video file
        public string GetVideoPath()
        {
            var videoPath = Path.Combine(AppContext.BaseDirectory, "Videos", "Snooker_Video.mp4");

            if (!File.Exists(videoPath))
            {
                throw new FileNotFoundException($"The video file cannot be found at path: {videoPath}");
            }

            return videoPath;
        }

        public async Task SetUpVideoCaptureAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    // Get the path to the video file
                    _videoPath = GetVideoPath();
                    _snookerVideo = new VideoCapture(_videoPath);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while accessing the video: {ex.Message}", ex);
                } 
            });
        }

        //Extract frame function
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
                if (_snookerVideo.IsOpened)
                {
                    Mat frame = new Mat();
                    double frameCount = 0;
                    double fps = this._snookerVideo.Get(CapProp.Fps);
                    //How many frame extracted per seconds
                    double frequency = 6;

                    while (true)
                    {
                        if (_cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            break;
                        }

                        _snookerVideo.Read(frame);

                        if (frame.IsEmpty)
                        {
                            break;
                        }

                        if (frameCount % (fps / frequency) == 0)
                        {
                            // Get the AppData directory path
                            string appDataDirectory = FileSystem.Current.AppDataDirectory;

                            // Create the full file path
                            string filePath = Path.Combine(appDataDirectory, "frame.jpg");

                            CvInvoke.Imwrite(filePath, frame);
                            //await Task.Delay(100);
                        }
                        frameCount++;
                    }
                }
            });
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

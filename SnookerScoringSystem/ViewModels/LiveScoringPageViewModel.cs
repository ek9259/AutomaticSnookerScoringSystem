using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SnookerScoringSystem.Domain;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.Views;

namespace SnookerScoringSystem.ViewModels
{
    public partial class LiveScoringPageViewModel : ObservableObject
    {
        private readonly IGetPlayerUseCase _getPlayerUseCase;
        private readonly IExtractFrameUseCase _extractFrameUseCase;
        private readonly IDetectSnookerBallUseCase _detectSnookerBallUseCase;
        private readonly IGetVideoPathUseCase _getVideoPathUseCase;

        private FileSystemWatcher _fileWatcher;
        private List<DetectedBall> _detectedBalls;
        
        [ObservableProperty]
        private Player _player1;

        [ObservableProperty]
        private Player _player2;

        [ObservableProperty] 
        private bool _isButtonVisible = true;

        [ObservableProperty] 
        private string _videoSource;


        public LiveScoringPageViewModel(IGetPlayerUseCase getPlayerUseCase, IExtractFrameUseCase extractFrameUseCase, 
            IDetectSnookerBallUseCase detectSnookerBallUseCase, IGetVideoPathUseCase getVideoPathUseCase)
        {
            this._getPlayerUseCase = getPlayerUseCase;
            this._player1 = new Player();
            this._player2 = new Player();
            this._detectedBalls = new List<DetectedBall>();

            this._getVideoPathUseCase = getVideoPathUseCase;
            this._extractFrameUseCase = extractFrameUseCase;
            this._detectSnookerBallUseCase = detectSnookerBallUseCase;

            setUpFileWatcher();
        }

        public async Task UpdatePlayer()
        {
            var players = await _getPlayerUseCase.ExecuteAsync();
            Player1 = players[0];
            Player2= players[1];
        }

        [RelayCommand]
        public async Task ExtractFrame()
        {
            playVideo();
            hideButton();
            await _extractFrameUseCase.ExecuteAsync();
        }

        private void playVideo()
        {
            try
            {
                VideoSource = _getVideoPathUseCase.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while loading the video", ex);
            }
        }

        private void setUpFileWatcher()
        {
            //Set up the FileSystemWatcher to keep track on the changes of extracted frame
            _fileWatcher = new FileSystemWatcher();
            _fileWatcher.Path = Path.Combine(FileSystem.Current.AppDataDirectory);
            _fileWatcher.Filter = "frame.jpg";
            _fileWatcher.Changed += OnFrameChanged;
            _fileWatcher.EnableRaisingEvents = true;
        }

        private void hideButton()
        {
            IsButtonVisible = false;
        }

        private async Task detectSnookerBall(string framePath)
        {
            _detectedBalls = await _detectSnookerBallUseCase.ExecuteAsync(framePath);

            int score = 0;
            foreach (var ball in _detectedBalls)
            {
                if (ball.ClassName == "Red ball")
                {
                    score++;
                }
            }
            Player1.Score = score;
        }

        private void OnFrameChanged(object sender, FileSystemEventArgs e)
        {
            // Get the path of the new frame
            string framePath = e.FullPath;

            // Detect the snooker ball
            detectSnookerBall(framePath);
        }
    }
}

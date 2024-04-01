using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mopups.Interfaces;
using SnookerScoringSystem.Domain;
using SnookerScoringSystem.Domain.Messages;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.Views.Popups;
using SnookerScoringSystem.Services.Intefaces;
using SnookerScoringSystem.GameplayServices.Interfaces;
using SnookerScoringSystem.GameplayServices;

namespace SnookerScoringSystem.ViewModels
{
    public partial class LiveScoringPageViewModel : ObservableObject
    {
        private readonly IGetPlayerUseCase _getPlayerUseCase;
        private readonly IExtractFrameUseCase _extractFrameUseCase;
        private readonly IDetectSnookerBallUseCase _detectSnookerBallUseCase;
        private readonly IGetVideoPathUseCase _getVideoPathUseCase;
        private readonly IUpdatePlayerScoreUseCase _updatePlayerScoreUseCase;
        private readonly IResetPlayerScoreUseCase _resetPlayerScoreUseCase;
        private readonly IStopExtractingFrameUseCase _stopExtractingFrameUseCase;
        private readonly ICalculateScore _calculateScore;

        private readonly IPopupNavigation _popupNavigation;
        private readonly ITimerService _timerService;

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

        [ObservableProperty] 
        private string _formattedMatchTime;


        [Obsolete]
        public LiveScoringPageViewModel(IGetPlayerUseCase getPlayerUseCase, IExtractFrameUseCase extractFrameUseCase, 
            IDetectSnookerBallUseCase detectSnookerBallUseCase, IGetVideoPathUseCase getVideoPathUseCase, IPopupNavigation popupNavigation,
            IUpdatePlayerScoreUseCase updatePlayerScoreUseCase, IResetPlayerScoreUseCase resetPlayerScoreUseCase, 
            IStopExtractingFrameUseCase stopExtractingFrameUseCase, ITimerService timerService, IGameManager gameManager)
        {
            this._getPlayerUseCase = getPlayerUseCase;
            this._player1 = new Player();
            this._player2 = new Player();
            this._detectedBalls = new List<DetectedBall>();

            this._getVideoPathUseCase = getVideoPathUseCase;
            this._extractFrameUseCase = extractFrameUseCase;
            this._detectSnookerBallUseCase = detectSnookerBallUseCase;
            this._updatePlayerScoreUseCase = updatePlayerScoreUseCase;
            this._resetPlayerScoreUseCase = resetPlayerScoreUseCase;
            this._stopExtractingFrameUseCase = stopExtractingFrameUseCase;

            this._popupNavigation = popupNavigation;
            this._timerService = timerService;

            this._timerService.TimeUpdated += UpdateFormattedMatchTime;

            _calculateScore = gameManager.StartNewGame();

            WeakReferenceMessenger.Default.Register<ResetPlayerScoreMessage>(this, (r, m) =>
            {
                Task.Run(() => ResetScore());
            });
            WeakReferenceMessenger.Default.Register<GoToScoreBoardPageMessage>(this, (r, m) =>
            {
                Device.InvokeOnMainThreadAsync(async () => await GoToNextPage());
            });

            this._timerService.Reset();
            SetUpFileWatcher();
        }


        public async Task UpdatePlayer()
        {
            var players = await _getPlayerUseCase.ExecuteAsync();
            Player1 = players[0];
            Player2= players[1];
        }

        [RelayCommand]
        private async Task ExtractFrame()
        {
            PlayVideo();
            HideButton();

            FormattedMatchTime = this._timerService.FormattedMatchTime;
            this._timerService.Start();

            await this._extractFrameUseCase.ExecuteAsync();

            _timerService.Stop();
        }

        [RelayCommand]
        private async Task OpenResetPopupPage()
        {
            try
            {
                await _popupNavigation.PushAsync(new ResetScorePopupPage(new ResetScorePopupPageViewModel()));
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while closing the popup page. {ex}");
            }
        }

        
        [RelayCommand]
        private async Task OpenEndGamePopupPage()
        {
            try
            {
                await _popupNavigation.PushAsync(new EndGamePopupPage(new EndGamePopupPageViewModel()));
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while closing the popup page. {ex}");
            }
        }


        private async Task ResetScore()
        {
            this._timerService.Reset();
            this._timerService.Start();

            var playerScore = await this._resetPlayerScoreUseCase.ExecuteAsync();
            Player1.Score = playerScore[0];
            Player2.Score = playerScore[1];
            _calculateScore.ResetScore();
        }

        private void PlayVideo()
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

        private void SetUpFileWatcher()
        {
            //Set up the FileSystemWatcher to keep track on the changes of extracted frame
            _fileWatcher = new FileSystemWatcher();
            _fileWatcher.Path = Path.Combine(FileSystem.Current.AppDataDirectory);
            _fileWatcher.Filter = "frame.jpg";
            _fileWatcher.Changed += OnFrameChanged;
            _fileWatcher.EnableRaisingEvents = true;
        }

        private void HideButton()
        {
            IsButtonVisible = false;
        }


        private async Task DetectSnookerBall(string framePath)
        {
            _detectedBalls = await _detectSnookerBallUseCase.ExecuteAsync(framePath);

            /* int score = 0;
             foreach (var ball in _detectedBalls)
             {
                 if (ball.ClassName == "Red ball")
                 {
                     score++;
                 }
             }
             Player1.Score = score;

             await this._updatePlayerScoreUseCase.ExecuteAsync(Player1.Score, Player2.Score); */
            List<int> PlayerScores = await _calculateScore.CalculateScoreAsync(_detectedBalls);
            await this._updatePlayerScoreUseCase.ExecuteAsync(PlayerScores[0], PlayerScores[1]);
        }

        private void OnFrameChanged(object sender, FileSystemEventArgs e)
        {
            // Get the path of the new frame
            string framePath = e.FullPath;

            // Detect the snooker ball
            Task.Run(() => DetectSnookerBall(framePath));
        }

        public void UpdateFormattedMatchTime()
        {
            FormattedMatchTime = this._timerService.FormattedMatchTime; 
        }

        public void ResetTimer()
        {
            FormattedMatchTime = this._timerService.FormattedMatchTime; 
        }
        private async Task GoToNextPage()
        {
            try
            {
                if (!IsButtonVisible)
                {
                    IsButtonVisible = true;
                }

                this._stopExtractingFrameUseCase.Execute();
                this._timerService.Stop();
                _calculateScore.ResetScore();
                VideoSource = "";
                await Shell.Current.GoToAsync($"{nameof(ScoreBoardPage)}");
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex}");
            }
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mopups.Interfaces;
using SnookerScoringSystem.Domain;
using SnookerScoringSystem.Domain.Messages;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.Views.Popups;
using SnookerScoringSystem.GameplayServices.Interfaces;
using SnookerScoringSystem.GameplayServices.PluginInterfaces;

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
        private readonly IFrameWatcherService _frameWatcherService;

        //private FileSystemWatcher _fileWatcher;
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
            IStopExtractingFrameUseCase stopExtractingFrameUseCase, ITimerService timerService, IGameManager gameManager, 
            IFrameWatcherService frameWatcherService)
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
            this._frameWatcherService = frameWatcherService;

            // Subscribe TimUpdated event to execute the UpdateFormmatedMatchTime to change the displaying match time
            this._timerService.TimeUpdated += UpdateFormattedMatchTime;

            this._calculateScore = gameManager.StartNewGame();


            // Register the ResetPlayerScoreMessage and once received it, execute ResetScore method.
            WeakReferenceMessenger.Default.Register<ResetPlayerScoreMessage>(this, (r, m) =>
            {
                Task.Run(() => ResetScore());
            });

            // Register the ResetPlayerScoreMessage and once received it, execute ResetScore method.
            WeakReferenceMessenger.Default.Register<GoToScoreBoardPageMessage>(this, (r, m) =>
            {
                Device.InvokeOnMainThreadAsync(async () => await GoToNextPage());
            });

            // Reset the match time to zero before starting the game
            this._timerService.Reset();

            // Call method to set up file watcher to keep track the changes of extracted frame
            this._frameWatcherService.FrameChanged += framePath =>
            {
                Task.Run(() => DetectSnookerBall(framePath));
            };
        }

        // Update player object to be displayed be getting data from repository
        public async Task UpdatePlayer()
        {
            var players = await _getPlayerUseCase.ExecuteAsync();
            Player1 = players[0];
            Player2= players[1];
        }

        // When user  click the start button, start extracting frame, playing video and start timer
        // Stop timer while the executing frame command is  interrupted or end.
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


        // While user click on reset button, pop up to ask user whether they are really want to do that
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

        // Before ending the game, make sure the user really want to do so by popping up confirmation message
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


        // Reset player score and timer
        private async Task ResetScore()
        {
            this._timerService.Reset();
            this._timerService.Start();

            var playerScore = await this._resetPlayerScoreUseCase.ExecuteAsync();
            Player1.Score = playerScore[0];
            Player2.Score = playerScore[1];
            _calculateScore.ResetScore();
        }

        // Start playing video by setting the video source
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

        // Hide the "Start" button after user clicked it
        private void HideButton()
        {
            IsButtonVisible = false;
        }

        // Start detecting snooker ball 
        private async Task DetectSnookerBall(string framePath)
        {
            _detectedBalls = await _detectSnookerBallUseCase.ExecuteAsync(framePath);

            List<int> PlayerScores = await _calculateScore.CalculateScoreAsync(_detectedBalls);
            await this._updatePlayerScoreUseCase.ExecuteAsync(PlayerScores[0], Player1.Foul, PlayerScores[1], Player2.Foul);
        }

        // Method to be called when the timer is elapsed with 1 second
        public void UpdateFormattedMatchTime()
        {
            FormattedMatchTime = this._timerService.FormattedMatchTime; 
        }

        // Redirect to next page, and stop the timer, extracting frame as well as reset calculation
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

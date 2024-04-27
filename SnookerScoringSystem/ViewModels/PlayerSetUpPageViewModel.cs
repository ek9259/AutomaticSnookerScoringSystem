using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Interfaces;
using SnookerScoringSystem.Views;
using SnookerScoringSystem.Domain;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.Views.Popups;

namespace SnookerScoringSystem.ViewModels
{
    public partial class PlayerSetUpPageViewModel : ObservableObject
    {
        private readonly IAddPlayerUseCase _addPlayerUseCase;
        private readonly IPopupNavigation _popupNavigation;
        private readonly ISetUpVideoCaptureUseCase _setupVideoCaptureUseCase;
     
        [ObservableProperty]
        private Player? _player1;

        [ObservableProperty]
        private Player? _player2;

        [ObservableProperty] 
        private bool _isButtonVisible = true;

        [ObservableProperty] 
        private bool _isIndicatorVisible = false;

        public PlayerSetUpPageViewModel(IAddPlayerUseCase addPlayerUseCase, IPopupNavigation popupNavigation, ISetUpVideoCaptureUseCase setUpVideoCaptureUseCase)
        {
            this._addPlayerUseCase = addPlayerUseCase;
            this._popupNavigation = popupNavigation;
            this._setupVideoCaptureUseCase = setUpVideoCaptureUseCase;
        }

        [RelayCommand]
        private async Task AddPlayer()
        {
            IsIndicatorVisible = true;
            IsButtonVisible = false;
            await this._addPlayerUseCase.ExecuteAsync(this.Player1);
            await this._addPlayerUseCase.ExecuteAsync(this.Player2);

            //Set up the camera capture before navigating to next page
            await this._setupVideoCaptureUseCase.ExecuteAsync();
            await Shell.Current.GoToAsync($"{nameof(LiveScoringPage)}");
        }

        public void ResetPlayers()
        {
            IsIndicatorVisible = false;
            IsButtonVisible = true;
            Player1 = new Player();
            Player2 = new Player();
        }

    }
}

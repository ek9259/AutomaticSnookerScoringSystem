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

        [ObservableProperty]
        private Player? _player1;

        [ObservableProperty]
        private Player? _player2;


        public PlayerSetUpPageViewModel(IAddPlayerUseCase addPlayerUseCase, IPopupNavigation popupNavigation)
        {
            this._addPlayerUseCase = addPlayerUseCase;
            this._popupNavigation = popupNavigation;
        }

        [RelayCommand]
        private async Task AddPlayer()
        {
            await this._addPlayerUseCase.ExecuteAsync(this.Player1);
            await this._addPlayerUseCase.ExecuteAsync(this.Player2);


            await Shell.Current.GoToAsync($"{nameof(LiveScoringPage)}");
        }

        public void ResetPlayers()
        {
            Player1 = new Player();
            Player2 = new Player();
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SnookerScoringSystem.Views;
using SnookerScoringSystem.Domain;
using SnookerScoringSystem.UseCases.Interfaces;

namespace SnookerScoringSystem.ViewModels
{
    public partial class PlayerSetUpPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private Player? _player1;

        [ObservableProperty]
        private Player? _player2;

        private readonly IAddPlayerUseCase _addPlayerUseCase;


        public PlayerSetUpPageViewModel(IAddPlayerUseCase addPlayerUseCase)
        {
            this.Player1 = new Player();
            this.Player2 = new Player(); 
            this._addPlayerUseCase = addPlayerUseCase;
        }

        [RelayCommand]
        public async Task AddPlayer()
        {
            await this._addPlayerUseCase.ExecuteAsync(this.Player1);
            await this._addPlayerUseCase.ExecuteAsync(this.Player2);

            await Shell.Current.GoToAsync($"{nameof(LiveScoringPage)}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SnookerScoringSystem.Domain;
using SnookerScoringSystem.UseCases.Interfaces;

namespace SnookerScoringSystem.ViewModels
{
    public partial class LiveScoringPageViewModel : ObservableObject
    {
        private readonly IGetPlayerUseCase _getPlayerUseCase;
        
        [ObservableProperty]
        private Player _player1;

        [ObservableProperty]
        private Player _player2;

        public LiveScoringPageViewModel(IGetPlayerUseCase getPlayerUseCase)
        {
            this._getPlayerUseCase = getPlayerUseCase;
            this._player1 = new Player();
            this._player2 = new Player();
        }

        public async Task UpdatePlayer()
        {
            var players = await _getPlayerUseCase.ExecuteAsync();
            Player1 = players[0];
            Player2= players[1];
        }
    }
}

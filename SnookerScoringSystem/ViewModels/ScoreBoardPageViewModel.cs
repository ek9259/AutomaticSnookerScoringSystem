using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SnookerScoringSystem.GameplayServices.PluginInterfaces;
using SnookerScoringSystem.Domain.Messages;
using CommunityToolkit.Mvvm.Messaging;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.Views;

namespace SnookerScoringSystem.ViewModels
{
    public partial class ScoreBoardPageViewModel : ObservableObject
    {
        [ObservableProperty] 
        private string _formattedMatchTime;

        [ObservableProperty] 
        private int _player1Score;

        [ObservableProperty] 
        private string? _player1Name;

        [ObservableProperty]
        private int _player2Score;

        [ObservableProperty] 
        private string? _player2Name;

        private readonly IGetPlayerUseCase _getPlayerUseCase;
        private readonly IResetPlayersUseCase _resetPlayersUseCase;
        private readonly ITimerService _timerService;


        public ScoreBoardPageViewModel(IGetPlayerUseCase getPlayerUseCase, IResetPlayersUseCase resetPlayersUseCase, ITimerService timerService)
        {
            this._getPlayerUseCase = getPlayerUseCase;
            this._resetPlayersUseCase = resetPlayersUseCase;
            this._timerService = timerService;

            WeakReferenceMessenger.Default.Register<OpeningScoreBoardPageMessage>(this, (r, m) =>
            {
                Task.Run(() => GetPlayers());
                FormattedMatchTime = this._timerService.FormattedMatchTime;
            });
        }      

        [RelayCommand]
        private async Task StartNewGame()
        {
            try
            {
                this._resetPlayersUseCase.Execute();
                this._timerService.Reset();
                await Shell.Current.GoToAsync("///PlayerSetUpPage");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured while starting a new game. {ex}");
            }
        }

        private async Task GetPlayers()
        {
            var players = await this._getPlayerUseCase.ExecuteAsync();
            Player1Score = players[0].Score;
            Player1Name = players[0].Name;
            Player2Score = players[1].Score;
            Player2Name = players[1].Name;
        }


    }
}

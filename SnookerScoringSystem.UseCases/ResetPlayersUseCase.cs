using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.UseCases
{
    public class ResetPlayersUseCase : IResetPlayersUseCase
    {
        private readonly IPlayerRepository _playerRepository;
        public ResetPlayersUseCase(IPlayerRepository playerRepository)
        {
            this._playerRepository = playerRepository;
        }
        public void Execute()
        {
            this._playerRepository.Reset();
        }
    }
}

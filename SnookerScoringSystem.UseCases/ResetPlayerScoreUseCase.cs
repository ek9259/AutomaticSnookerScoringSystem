using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.UseCases
{
    public class ResetPlayerScoreUseCase : IResetPlayerScoreUseCase
    {
        private readonly IPlayerRepository _playerRepository;

        public ResetPlayerScoreUseCase(IPlayerRepository playerRepository)
        {
            this._playerRepository = playerRepository;
        }

        public async Task<List<int>> ExecuteAsync()
        {
            return await this._playerRepository.ResetPlayerScoreAsync();
        }
    }
}

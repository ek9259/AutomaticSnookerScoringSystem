using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnookerScoringSystem.Domain;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.UseCases
{
    public class GetPlayerUseCase : IGetPlayerUseCase
    {
        private readonly IPlayerRepository _playerRepository;

        public GetPlayerUseCase(IPlayerRepository playerRepository)
        {
            this._playerRepository = playerRepository;
        }
        public async Task<List<Player>> ExecuteAsync()
        {
            return await this._playerRepository.GetPlayerAsync();
        }
    }
}

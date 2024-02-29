using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.UseCases.PluginInterfaces;
using Player = SnookerScoringSystem.Domain.Player;

namespace SnookerScoringSystem.UseCases
{
    // All the code in this file is included in all platforms.
    public class AddPlayerUseCase : IAddPlayerUseCase
    {
        private readonly IPlayerRepository _playerRepository;

        public AddPlayerUseCase(IPlayerRepository playerRepository)
        {
            this._playerRepository = playerRepository;
        }
        public async Task ExecuteAsync(Player player)
        {
            await this._playerRepository.AddPlayerAsync(player);
        }
    }
}

using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.UseCases.PluginInterfaces;
using Player = SnookerScoringSystem.Domain.Player;

namespace SnookerScoringSystem.UseCases
{
    // This class implements the IAddPlayerUseCase interface and defines how to add a player to the system.
    public class AddPlayerUseCase : IAddPlayerUseCase
    {
        private readonly IPlayerRepository _playerRepository;

        public AddPlayerUseCase(IPlayerRepository playerRepository)
        {
            this._playerRepository = playerRepository;
        }

        // Calling the AddPlayerAsync method of the player repository to add the player to the system.
        public async Task ExecuteAsync(Player player)
        {
            await this._playerRepository.AddPlayerAsync(player);
        }
    }
}

using SnookerScoringSystem.Domain;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.UseCases
{
    // This class implements the IGetPlayerUseCase interface and defines how to get a player from the repository.
    public class GetPlayerUseCase : IGetPlayerUseCase
    {
        private readonly IPlayerRepository _playerRepository;

        public GetPlayerUseCase(IPlayerRepository playerRepository)
        {
            this._playerRepository = playerRepository;
        }

        // Calling the GetPlayerAsync method of the player repository to get the player from the repository.
        public async Task<List<Player>> ExecuteAsync()
        {
            return await this._playerRepository.GetPlayerAsync();
        }
    }
}

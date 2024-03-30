using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.UseCases
{
    public class UpdatePlayerScoreUseCase : IUpdatePlayerScoreUseCase
    {
        private readonly IPlayerRepository _playerRepository;

        public UpdatePlayerScoreUseCase(IPlayerRepository playerRepository)
        {
            this._playerRepository = playerRepository;
        }

        public async Task ExecuteAsync(int player1Score, int player2Score)
        {
            await this._playerRepository.UpdatePlayerScoreAsync(player1Score, player2Score);
        }
    }
}

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

        public async Task ExecuteAsync(int player1Score, int player1Foul, int player2Score, int player2Foul)
        {
            await this._playerRepository.UpdatePlayerScoreAsync(player1Score, player1Foul, player2Score, player2Foul);
        }
    }
}

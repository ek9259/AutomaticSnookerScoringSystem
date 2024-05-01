using SnookerScoringSystem.GameplayServices.Interfaces;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.GameplayServices.PluginInterfaces;
using SnookerScoringSystem.UseCases;

namespace SnookerScoringSystem.GameplayServices
{
    public class GameManager : IGameManager
    {
        public ICalculateScore StartNewGame(IUpdatePlayerScoreUseCase updatePlayerScoreUseCase, IGetPlayerUseCase getPlayerUseCase)
        {

            return new CalculateScore(updatePlayerScoreUseCase, getPlayerUseCase);
        }
    }
}

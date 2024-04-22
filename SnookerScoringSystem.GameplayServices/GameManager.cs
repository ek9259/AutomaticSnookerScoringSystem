using SnookerScoringSystem.GameplayServices.Interfaces;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.GameplayServices.PluginInterfaces;

namespace SnookerScoringSystem.GameplayServices
{
    public class GameManager : IGameManager
    {
        public ICalculateScore StartNewGame(IUpdatePlayerScoreUseCase updatePlayerScoreUseCase)
        {

            return new CalculateScore(updatePlayerScoreUseCase);
        }
    }
}

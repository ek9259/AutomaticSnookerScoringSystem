using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.GameplayServices.Interfaces;

namespace SnookerScoringSystem.GameplayServices.PluginInterfaces
{
    public interface IGameManager
    {
        ICalculateScore StartNewGame(IUpdatePlayerScoreUseCase updatePlayerScoreUseCase);
    }
}

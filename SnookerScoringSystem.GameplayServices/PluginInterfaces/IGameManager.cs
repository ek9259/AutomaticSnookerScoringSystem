using SnookerScoringSystem.Domain;
using SnookerScoringSystem.GameplayServices.Interfaces;

namespace SnookerScoringSystem.GameplayServices.PluginInterfaces
{
    public interface IGameManager
    {
        ICalculateScore StartNewGame();
    }
}

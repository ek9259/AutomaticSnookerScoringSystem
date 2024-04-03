using SnookerScoringSystem.Domain;

namespace SnookerScoringSystem.GameplayServices.Interfaces
{
    public interface IGameManager
    {
        ICalculateScore StartNewGame();
    }
}

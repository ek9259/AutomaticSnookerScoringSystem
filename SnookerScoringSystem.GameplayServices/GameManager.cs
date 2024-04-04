using SnookerScoringSystem.GameplayServices.Interfaces;
using SnookerScoringSystem.GameplayServices.PluginInterfaces;

namespace SnookerScoringSystem.GameplayServices
{
    public class GameManager : IGameManager
    {
        public ICalculateScore StartNewGame()
        {

            return new CalculateScore();
        }
    }
}

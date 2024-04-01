using SnookerScoringSystem.GameplayServices.Interfaces;
using SnookerScoringSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

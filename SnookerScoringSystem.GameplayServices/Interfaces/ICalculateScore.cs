using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Player = SnookerScoringSystem.Domain.Player;
using SnookerScoringSystem.Domain;

namespace SnookerScoringSystem.GameplayServices.Interfaces
{
    public interface ICalculateScore
    {
        Task <List<int>> CalculateScoreAsync(List<DetectedBall> detectedBalls);
    }
}

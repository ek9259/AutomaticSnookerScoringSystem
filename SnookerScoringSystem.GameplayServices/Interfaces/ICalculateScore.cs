using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SnookerScoringSystem.Domain;

namespace SnookerScoringSystem.GameplayServices.Interfaces
{
    public interface ICalculateScore
    {
        void Reset();
        Task CalculateScoreAsync(List<DetectedBall> detectedBalls);
    }
}

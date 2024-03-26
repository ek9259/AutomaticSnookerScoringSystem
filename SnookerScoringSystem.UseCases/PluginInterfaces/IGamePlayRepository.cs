using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnookerScoringSystem.Domain;

namespace SnookerScoringSystem.UseCases.PluginInterfaces
{
    public interface IGamePlayRepository
    {
        Task CalculateScoreAsync(List<DetectedBall> detectedBalls);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerScoringSystem.UseCases.PluginInterfaces
{
    public interface ISnookerDetectionModelRepository
    {
        Task DetectSnookerBallAsync(string framePath);

    }
}

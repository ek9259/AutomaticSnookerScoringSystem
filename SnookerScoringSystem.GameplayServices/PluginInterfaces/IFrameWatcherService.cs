using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerScoringSystem.GameplayServices.PluginInterfaces
{
    public interface IFrameWatcherService
    {
        event Action<string> FrameChanged;

    }
}

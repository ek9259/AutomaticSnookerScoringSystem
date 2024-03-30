using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerScoringSystem.Services.Intefaces
{
    public interface ITimerService
    {
        event Action TimeUpdated;
        TimeSpan MatchTime { get; }
        string FormattedMatchTime { get; set; }
        void Start();
        void Stop();
        void Reset();
    }
}

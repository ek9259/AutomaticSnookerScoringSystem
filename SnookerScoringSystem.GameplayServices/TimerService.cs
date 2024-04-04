using System;
using System.Timers;
using SnookerScoringSystem.GameplayServices.Interfaces;
using SnookerScoringSystem.GameplayServices.PluginInterfaces;
using Timer = System.Timers.Timer;


namespace SnookerScoringSystem.GameplayServices
{
    public class TimerService : ITimerService
    {
        private Timer _timer;
        private TimeSpan _matchTime;

        public event Action TimeUpdated;

        public TimeSpan MatchTime
        {
            get { return _matchTime; }
            private set { _matchTime = value; }
        }

        //Return match time in formatted string
        public string FormattedMatchTime
        {
            get { return MatchTime.ToString(@"hh\:mm\:ss"); }
            set { MatchTime.ToString(@"hh\:mm\:ss"); }
        }

        //Constructor
        public TimerService()
        {
            _timer = new Timer(1000);
            //Subscribe the timer.Elapsed to OnTimerElapsed, it will raise that event while one second passed
            _timer.Elapsed += OnTimerElapsed;
            MatchTime = TimeSpan.Zero;
        }

        // When the event is raised, add 1 second to MatchTime and notify other components that the time is updated
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            MatchTime = MatchTime.Add(TimeSpan.FromSeconds(1));
            TimeUpdated?.Invoke();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Reset()
        {
            MatchTime = TimeSpan.Zero;
            _timer.Stop();
        }
    }
}

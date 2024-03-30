using System;
using System.Timers;
using SnookerScoringSystem.Services.Intefaces;
using Timer = System.Timers.Timer;

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

    public string FormattedMatchTime
    {
        get { return MatchTime.ToString(@"hh\:mm\:ss"); }
        set { MatchTime.ToString(@"hh\:mm\:ss"); }
    }

    public TimerService()
    {
        _timer = new Timer(1000);
        _timer.Elapsed += OnTimerElapsed;
        MatchTime = TimeSpan.Zero;
    }



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
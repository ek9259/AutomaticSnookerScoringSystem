using SnookerScoringSystem.Views;
using SnookerScoringSystem.Views.Popups;

namespace SnookerScoringSystem
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PlayerSetUpPage), typeof(PlayerSetUpPage));
            Routing.RegisterRoute(nameof(LiveScoringPage), typeof(LiveScoringPage));
            Routing.RegisterRoute(nameof(ScoreBoardPage), typeof(ScoreBoardPage));
        }
    }
}

using SnookerScoringSystem.Views;

namespace SnookerScoringSystem
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LiveScoringPage), typeof(LiveScoringPage));
        }
    }
}

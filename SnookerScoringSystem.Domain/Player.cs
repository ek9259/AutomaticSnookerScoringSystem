using CommunityToolkit.Mvvm.ComponentModel;

namespace SnookerScoringSystem.Domain
{
    public partial class Player : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private int _score;

        [ObservableProperty]
        private int _foul;
    }
}
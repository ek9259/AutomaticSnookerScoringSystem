using CommunityToolkit.Mvvm.ComponentModel;

namespace SnookerScoringSystem.Domain
{
    public partial class Player : ObservableObject
    {
        [ObservableProperty]
        private int _Id;

        [ObservableProperty]
        private string _Name;

        [ObservableProperty]
        private int _Score;

        [ObservableProperty]
        private int _Foul;
        //public int Id { get; set; }

        //public string? Name
        //{
        //    get => _Name;
        //    set => SetProperty(ref _Name, value);
        //}

        //public int Score 
        //{        
        //    get => _Score;
        //    set => SetProperty(ref _Score, value);
        //}

        //public int Foul
        //{        
        //    get => _Foul;
        //    set => SetProperty(ref _Foul, value);
        //}
    }
}
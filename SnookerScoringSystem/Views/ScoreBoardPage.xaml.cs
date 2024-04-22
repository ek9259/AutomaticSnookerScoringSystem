using SnookerScoringSystem.ViewModels;

namespace SnookerScoringSystem.Views.Popups;

public partial class ScoreBoardPage : ContentPage
{
    private readonly ScoreBoardPageViewModel _scoreBoardPageViewModel; 
    public ScoreBoardPage(ScoreBoardPageViewModel scoreBoardPageViewModel)
    {
        InitializeComponent();
        this._scoreBoardPageViewModel = scoreBoardPageViewModel;
        BindingContext = this._scoreBoardPageViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this._scoreBoardPageViewModel.GetPlayers();
        this._scoreBoardPageViewModel.UpdateMatchTime();
    }
}
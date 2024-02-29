using SnookerScoringSystem.ViewModels;

namespace SnookerScoringSystem.Views;

public partial class LiveScoringPage : ContentPage
{
    private readonly LiveScoringPageViewModel _liveScoringPageViewModel;
	public LiveScoringPage(LiveScoringPageViewModel liveScoringPageViewModel)
	{
        InitializeComponent();
        this._liveScoringPageViewModel = liveScoringPageViewModel;
        BindingContext = this._liveScoringPageViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await this._liveScoringPageViewModel.UpdatePlayer();
    }
}
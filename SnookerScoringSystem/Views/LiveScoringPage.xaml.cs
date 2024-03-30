using Mopups.Interfaces;
using SnookerScoringSystem.ViewModels;
using SnookerScoringSystem.Views.Popups;

namespace SnookerScoringSystem.Views;

public partial class LiveScoringPage : ContentPage
{
    private readonly LiveScoringPageViewModel _liveScoringPageViewModel;
    private readonly IPopupNavigation _popupNavigation;
    public LiveScoringPage(LiveScoringPageViewModel liveScoringPageViewModel, IPopupNavigation popupNavigation)
    {
        InitializeComponent();
        this._liveScoringPageViewModel = liveScoringPageViewModel;
        this._popupNavigation = popupNavigation;
        BindingContext = this._liveScoringPageViewModel;

    }


    //While this page is redirected, open the main popup page and update player name and score
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _popupNavigation.PushAsync(new MainPopupPage(new MainPopupPageViewModel()));
        await this._liveScoringPageViewModel.UpdatePlayer();
        this._liveScoringPageViewModel.UpdateFormattedMatchTime();
    }
}
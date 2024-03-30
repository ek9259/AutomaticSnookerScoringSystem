using CommunityToolkit.Mvvm.Messaging;
using Mopups.Pages;
using Mopups.Services;
using SnookerScoringSystem.Domain.Messages;
using SnookerScoringSystem.ViewModels;

namespace SnookerScoringSystem.Views.Popups;

public partial class EndGamePopupPage : PopupPage
{
    private readonly EndGamePopupPageViewModel _endGamePopupPageViewModel;
    public EndGamePopupPage(EndGamePopupPageViewModel endGamePopupPageViewModel) 
    { 
        InitializeComponent();
        this._endGamePopupPageViewModel = endGamePopupPageViewModel;
        BindingContext = this._endGamePopupPageViewModel;
    }

}
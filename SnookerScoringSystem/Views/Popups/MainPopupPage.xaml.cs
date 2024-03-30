using CommunityToolkit.Mvvm.Messaging;
using Mopups.Pages;
using Mopups.Services;
using SnookerScoringSystem.Domain.Messages;
using SnookerScoringSystem.ViewModels;

namespace SnookerScoringSystem.Views.Popups;

public partial class MainPopupPage : PopupPage
{
    private readonly MainPopupPageViewModel _mainPopupPageViewModel;
    public MainPopupPage(MainPopupPageViewModel mainPopupPageViewModel) 
    { 
        InitializeComponent();
        this._mainPopupPageViewModel = mainPopupPageViewModel;
        BindingContext = this._mainPopupPageViewModel;
    }
}
using CommunityToolkit.Mvvm.Messaging;
using Mopups.Pages;
using Mopups.Services;
using SnookerScoringSystem.Domain.Messages;
using SnookerScoringSystem.ViewModels;

namespace SnookerScoringSystem.Views.Popups;

public partial class ResetScorePopupPage : PopupPage
{
    private readonly ResetScorePopupPageViewModel _resetScorePopupPageViewModel;
    public ResetScorePopupPage(ResetScorePopupPageViewModel resetScorePopupPageViewModel) 
    { 
        InitializeComponent();
        this._resetScorePopupPageViewModel = resetScorePopupPageViewModel;
        BindingContext = this._resetScorePopupPageViewModel;
    }
}
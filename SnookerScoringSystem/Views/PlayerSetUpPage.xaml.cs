using SnookerScoringSystem.ViewModels;
using UraniumUI.Pages;

namespace SnookerScoringSystem.Views;

public partial class PlayerSetUpPage : UraniumContentPage
{
    private readonly PlayerSetUpPageViewModel _playerSetUpPageViewModel;
    public PlayerSetUpPage(PlayerSetUpPageViewModel playerSetUpPageViewModel)
    {
        InitializeComponent();
        this._playerSetUpPageViewModel = playerSetUpPageViewModel;
        BindingContext = this._playerSetUpPageViewModel;
    }
}
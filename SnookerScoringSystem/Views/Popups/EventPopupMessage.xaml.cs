using Mopups.Pages;
using Mopups.Services;
using Syncfusion.Licensing.crypto;

namespace SnookerScoringSystem.Views.Popups;

public partial class EventPopupMessage : PopupPage
{ 
    public EventPopupMessage() 
    { 
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(TimeSpan.FromSeconds(4));
        await MopupService.Instance.PopAsync();
    }
}
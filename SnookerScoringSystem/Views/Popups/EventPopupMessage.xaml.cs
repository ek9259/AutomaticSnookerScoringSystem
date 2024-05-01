using Mopups.Pages;
using Mopups.Services;
using Microsoft.Maui.Graphics;
using SnookerScoringSystem.ViewModels;
using Syncfusion.Licensing.crypto;

namespace SnookerScoringSystem.Views.Popups;

public partial class EventPopupMessage : PopupPage
{
    public string Messages1 { get; set; }
    public string ColoredText { get; set; }
    public string Messages2 { get; set; }
    public Color BallColors { get; set; }

    private void ClassidToColors(int classid)
    {
        switch (classid)
        {
            case 0:
                BallColors = Colors.Red;
                break;
            case 1:
                BallColors = Colors.Yellow;
                break;
            case 2:
                BallColors = Colors.Green;
                break;
            case 3:
                BallColors = Colors.Brown;
                break;
            case 4:
                BallColors = Colors.White;
                break;
            case 5:
                BallColors = Colors.Blue;
                break;
            case 6:
                BallColors = Colors.Pink;
                break;
            case 7:
                BallColors = Colors.Black;
                break;
            default:
                BallColors = Colors.White;
                break;

        }
    }

    public EventPopupMessage(string messages1, string coloredtext, string messages2, int classid) 
    { 
        InitializeComponent();
        Messages1 = messages1;
        Messages2 = messages2;
        ColoredText = coloredtext;
        ClassidToColors(classid);
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(TimeSpan.FromSeconds(4));
        await MopupService.Instance.PopAsync();
    }
}
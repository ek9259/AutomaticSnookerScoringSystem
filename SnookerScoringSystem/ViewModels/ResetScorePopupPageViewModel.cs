using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mopups.Services;
using SnookerScoringSystem.Domain.Messages;

namespace SnookerScoringSystem.ViewModels
{
    public partial class ResetScorePopupPageViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task ClosePopupPage()
        {
            await MopupService.Instance.PopAsync();
        }

        [RelayCommand]
        private async Task SendResetScoreMessage()
        {
            WeakReferenceMessenger.Default.Send(new ResetPlayerScoreMessage("Reset"));
            await MopupService.Instance.PopAsync();
        }
    }
}

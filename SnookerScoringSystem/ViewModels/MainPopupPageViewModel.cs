using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;

namespace SnookerScoringSystem.ViewModels
{
    public partial class MainPopupPageViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task ClosePopupPage()
        {
            await MopupService.Instance.PopAsync();
        }
    }
}

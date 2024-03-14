using Microsoft.Extensions.Logging;
using SnookerScoringSystem.UseCases.PluginInterfaces;
using SnookerScoringSystem.Views;
using SnookerScoringSystem.Plugins.Datastore.InMemory;
using SnookerScoringSystem.UseCases;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.ViewModels;
using CommunityToolkit.Maui;
using UraniumUI;

namespace SnookerScoringSystem
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitMediaElement()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            // Registering the necessary services with the dependency injection container.
            builder.Services.AddSingleton<IPlayerRepository, PlayerInMemoryRepository>();
            //builder.Services.AddSingleton<ISnookerDetectionModelRepository, SnookerDetectionModelRepository>();
            builder.Services.AddTransient<IAddPlayerUseCase, AddPlayerUseCase>();
            builder.Services.AddTransient<IGetPlayerUseCase, GetPlayerUseCase>();

            // Registering the view models and views with the dependency injection container.
            builder.Services.AddSingleton<PlayerSetUpPageViewModel>();
            builder.Services.AddSingleton<LiveScoringPageViewModel>();

            builder.Services.AddSingleton<PlayerSetUpPage>();
            builder.Services.AddSingleton<LiveScoringPage>();
            return builder.Build();
        }
    }
}

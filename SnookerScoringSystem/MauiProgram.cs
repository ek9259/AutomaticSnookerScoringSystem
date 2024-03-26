using Microsoft.Extensions.Logging;
using SnookerScoringSystem.UseCases.PluginInterfaces;
using SnookerScoringSystem.Views;
using SnookerScoringSystem.Plugins.Datastore.InMemory;
using SnookerScoringSystem.Plugins.Datastore.Model;
using SnookerScoringSystem.Plugins.Datastore.VideoProcessing;
using SnookerScoringSystem.UseCases;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.ViewModels;
using CommunityToolkit.Maui;
using SnookerScoringSystem.Plugins.Datastore.GamePlay;
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
            builder.Services.AddSingleton<ISnookerDetectionModelRepository, ModelRepository>();
            builder.Services.AddSingleton<IVideoProcessingRepository, VideoProcessingRepository>();
            builder.Services.AddSingleton<IGamePlayRepository, GamePlayRepository>();
            builder.Services.AddTransient<IAddPlayerUseCase, AddPlayerUseCase>();
            builder.Services.AddTransient<IGetPlayerUseCase, GetPlayerUseCase>();
            builder.Services.AddTransient<IDetectSnookerBallUseCase, DetectSnookerBallUseCase>();
            builder.Services.AddTransient<IExtractFrameUseCase, ExtractFrameUseCase>();
            builder.Services.AddTransient<ICalculateScoreUseCase, CalculateScoreUseCase>();
            builder.Services.AddTransient<IGetVideoPathUseCase, GetVideoPathUseCase>();

            // Registering the view models and views with the dependency injection container.
            builder.Services.AddSingleton<PlayerSetUpPageViewModel>();
            builder.Services.AddSingleton<LiveScoringPageViewModel>();

            builder.Services.AddSingleton<PlayerSetUpPage>();
            builder.Services.AddSingleton<LiveScoringPage>();
            return builder.Build();
        }
    }
}

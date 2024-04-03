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
using Mopups.Hosting;
using Mopups.Interfaces;
using Mopups.Services;
using SnookerScoringSystem.Plugins.Datastore.GamePlay;
using SnookerScoringSystem.Services;
using SnookerScoringSystem.GameplayServices;
using SnookerScoringSystem.GameplayServices.Interfaces;
using SnookerScoringSystem.Services.Intefaces;
using SnookerScoringSystem.Views.Popups;
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
                .ConfigureMopups()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Fontspring-DEMO-anona-black.otf", "AnonaBlack");
                    fonts.AddFont("Fontspring-DEMO-anona-thin.otf", "AnonaThin");
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
            builder.Services.AddTransient<ICalculateScore, CalculateScore>();
            builder.Services.AddTransient<IGameManager, GameManager>();
            builder.Services.AddTransient<IGetVideoPathUseCase, GetVideoPathUseCase>();
            builder.Services.AddTransient<IUpdatePlayerScoreUseCase, UpdatePlayerScoreUseCase>();
            builder.Services.AddTransient<IResetPlayerScoreUseCase, ResetPlayerScoreUseCase>();
            builder.Services.AddTransient<IStopExtractingFrameUseCase, StopExtractingFrameUseCase>();
            builder.Services.AddTransient<IResetPlayersUseCase, ResetPlayersUseCase>();

            builder.Services.AddSingleton<ITimerService, TimerService>();

            // Registering the view models and views with the dependency injection container.
            builder.Services.AddSingleton<PlayerSetUpPageViewModel>();
            builder.Services.AddSingleton<LiveScoringPageViewModel>();
            builder.Services.AddSingleton<ScoreBoardPageViewModel>();
            builder.Services.AddSingleton<ResetScorePopupPageViewModel>();
            builder.Services.AddSingleton<EndGamePopupPageViewModel>();
            builder.Services.AddSingleton<MainPopupPageViewModel>();

            builder.Services.AddSingleton<PlayerSetUpPage>();
            builder.Services.AddSingleton<MainPopupPage>();
            builder.Services.AddSingleton<ResetScorePopupPage>();
            builder.Services.AddSingleton<EndGamePopupPage>();
            builder.Services.AddSingleton<ScoreBoardPage>();

            builder.Services.AddSingleton<IPopupNavigation>(MopupService.Instance);
            builder.Services.AddTransient<LiveScoringPage>();
            
            return builder.Build();
        }
    }
}

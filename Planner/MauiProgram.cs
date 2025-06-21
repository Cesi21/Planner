using Microsoft.Extensions.Logging;
using Planner.Services;
using Planner.ViewModels;
using Planner.Views;

namespace Planner
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<DataService>();
            builder.Services.AddTransient<GoalListViewModel>();
            builder.Services.AddTransient<RoutineListViewModel>();
            builder.Services.AddTransient<GoalListPage>();
            builder.Services.AddTransient<RoutineListPage>();

            return builder.Build();
        }
    }
}

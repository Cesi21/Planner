using Microsoft.Extensions.Logging;
using Planner.Services;
using Plugin.LocalNotification;
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
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<DataService>();
            builder.Services.AddSingleton<RoutineService>();
            builder.Services.AddSingleton<ReminderService>();
            builder.Services.AddTransient<GoalListViewModel>();
            builder.Services.AddTransient<RoutineListViewModel>();
            builder.Services.AddTransient<RoutineTemplateViewModel>();
            builder.Services.AddTransient<CalendarViewModel>();
            builder.Services.AddTransient<StatsViewModel>();
            builder.Services.AddTransient<GoalListPage>();
            builder.Services.AddTransient<RoutineListPage>();
            builder.Services.AddTransient<RoutineTemplatePage>();
            builder.Services.AddTransient<CalendarPage>();
            builder.Services.AddTransient<StatsPage>();

            return builder.Build();
        }
    }
}

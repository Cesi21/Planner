using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Planner.Services;

namespace Planner.ViewModels
{
    public partial class StatsViewModel : ObservableObject
    {
        private readonly RoutineService _routineService;

        [ObservableProperty]
        private string _weekRangeTitle = string.Empty;

        [ObservableProperty]
        private int _routinesCompleted;

        [ObservableProperty]
        private int _fullCompletionDays;

        [ObservableProperty]
        private double _completionPercentage;

        public StatsViewModel(RoutineService routineService)
        {
            _routineService = routineService;
        }

        public async Task LoadAsync()
        {
            var today = DateTime.Today;
            var start = today.AddDays(-(int)today.DayOfWeek);
            var end = start.AddDays(6);

            WeekRangeTitle = $"Week of {start:MMMM d} – {end:MMMM d}";

            var completed = await _routineService.GetTotalCompletedInRange(start, end);
            var planned = await _routineService.GetTotalPlannedInRange(start, end);
            var fullDays = await _routineService.GetFullCompletionDaysInRange(start, end);

            RoutinesCompleted = completed;
            FullCompletionDays = fullDays;
            CompletionPercentage = planned == 0 ? 0 : (double)completed / planned * 100.0;
        }
    }
}

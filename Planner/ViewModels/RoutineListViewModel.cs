using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Planner.Models;
using Planner.Services;

namespace Planner.ViewModels
{
    public partial class RoutineListViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        [ObservableProperty]
        private List<Routine> _routines = new();

        public RoutineListViewModel(DataService dataService)
        {
            _dataService = dataService;
        }

        public async Task LoadAsync()
        {
            Routines = await _dataService.GetRoutinesAsync();
        }

        [RelayCommand]
        private async Task ToggleRoutine(Routine routine)
        {
            if (routine.LastCompletedDate?.Date == DateTime.Today)
            {
                routine.LastCompletedDate = null;
                routine.StreakCount = Math.Max(0, routine.StreakCount - 1);
            }
            else
            {
                routine.LastCompletedDate = DateTime.Today;
                routine.StreakCount++;
            }

            await _dataService.UpdateRoutineAsync(routine);
            await LoadAsync();
        }

        [RelayCommand]
        private async Task AddRoutine()
        {
            var routine = new Routine { Name = "New Routine" };
            await _dataService.AddRoutineAsync(routine);
            await LoadAsync();
        }
    }
}

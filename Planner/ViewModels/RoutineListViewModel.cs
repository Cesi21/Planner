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
            Routines = await _dataService.GetTodayRoutinesAsync();
        }

        [RelayCommand]
        private async Task ToggleRoutine(Routine routine)
        {
            routine.IsCompleted = !routine.IsCompleted;
            await _dataService.UpdateTodayRoutineAsync(routine);
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

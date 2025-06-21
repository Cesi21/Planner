using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Planner.Models;
using Planner.Services;

namespace Planner.ViewModels
{
    public partial class RoutineListViewModel : ObservableObject
    {
        private readonly RoutineService _routineService;

        [ObservableProperty]
        private List<Routine> _routines = new();

        public RoutineListViewModel(RoutineService routineService)
        {
            _routineService = routineService;
        }

        public async Task LoadAsync()
        {
            await _routineService.GenerateDailyRoutinesFromTemplates(DateTime.Today);
            Routines = await _routineService.GetRoutinesByDate(DateTime.Today);
        }

        [RelayCommand]
        private async Task ToggleRoutine(Routine routine)
        {
            routine.IsCompleted = !routine.IsCompleted;
            routine.Date = DateTime.Today;
            await _routineService.SaveRoutineForDate(routine);
            await LoadAsync();
        }

        [RelayCommand]
        private async Task AddRoutine()
        {
            var name = await Shell.Current.DisplayPromptAsync("New Routine", "Name of the routine?");
            if (string.IsNullOrWhiteSpace(name))
                return;
            var routine = new Routine { Name = name, Date = DateTime.Today };
            await _routineService.SaveRoutineForDate(routine);
            await LoadAsync();
        }

        [RelayCommand]
        private async Task EditRoutine(Routine routine)
        {
            var name = await Shell.Current.DisplayPromptAsync("Edit Routine", "Routine name", initialValue: routine.Name);
            if (string.IsNullOrWhiteSpace(name))
                return;
            routine.Name = name;
            routine.Date = DateTime.Today;
            await _routineService.SaveRoutineForDate(routine);
            await LoadAsync();
        }

        [RelayCommand]
        private async Task DeleteRoutine(Routine routine)
        {
            await _routineService.DeleteRoutineAsync(routine.Id, DateTime.Today);
            await LoadAsync();
        }
    }
}

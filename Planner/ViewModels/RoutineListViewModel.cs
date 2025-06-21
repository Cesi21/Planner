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
        private readonly ReminderService _reminderService;

        [ObservableProperty]
        private List<Routine> _routines = new();

        public RoutineListViewModel(RoutineService routineService, ReminderService reminderService)
        {
            _routineService = routineService;
            _reminderService = reminderService;
        }

        public async Task LoadAsync()
        {
            await _routineService.GenerateDailyRoutinesFromTemplates(DateTime.Today);
            Routines = await _routineService.GetRoutinesByDate(DateTime.Today);
            await _reminderService.RescheduleTodayRemindersAsync();
        }

        [RelayCommand]
        private async Task ToggleRoutine(Routine routine)
        {
            routine.IsCompleted = !routine.IsCompleted;
            routine.Date = DateTime.Today;
            await _routineService.SaveRoutineForDate(routine);
            if (routine.IsCompleted)
                _reminderService.CancelReminder(routine.Id);
            else
                await _reminderService.ScheduleReminderAsync(routine);
            await LoadAsync();
        }

        [RelayCommand]
        private async Task AddRoutine()
        {
            var name = await Shell.Current.DisplayPromptAsync("New Routine", "Name of the routine?");
            if (string.IsNullOrWhiteSpace(name))
                return;
            bool enable = await Shell.Current.DisplayAlert("Reminder", "Enable reminder?", "Yes", "No");
            TimeSpan? time = null;
            if (enable)
            {
                var input = await Shell.Current.DisplayPromptAsync("Reminder Time", "Enter time (HH:mm)", initialValue: DateTime.Now.ToString("HH:mm"));
                if (TimeSpan.TryParse(input, out var ts))
                    time = ts;
            }

            var routine = new Routine { Name = name, Date = DateTime.Today, IsReminderEnabled = enable, ReminderTime = time };
            await _routineService.SaveRoutineForDate(routine);
            await _reminderService.ScheduleReminderAsync(routine);
            await LoadAsync();
        }

        [RelayCommand]
        private async Task EditRoutine(Routine routine)
        {
            var name = await Shell.Current.DisplayPromptAsync("Edit Routine", "Routine name", initialValue: routine.Name);
            if (string.IsNullOrWhiteSpace(name))
                return;
            routine.Name = name;
            bool enable = await Shell.Current.DisplayAlert("Reminder", "Enable reminder?", "Yes", "No");
            TimeSpan? time = routine.ReminderTime;
            if (enable)
            {
                var input = await Shell.Current.DisplayPromptAsync("Reminder Time", "Enter time (HH:mm)", initialValue: routine.ReminderTime?.ToString("hh\:mm") ?? DateTime.Now.ToString("HH:mm"));
                if (TimeSpan.TryParse(input, out var ts))
                    time = ts;
            }
            else
            {
                time = null;
            }

            routine.Date = DateTime.Today;
            routine.IsReminderEnabled = enable;
            routine.ReminderTime = time;
            await _routineService.SaveRoutineForDate(routine);
            if (routine.IsReminderEnabled)
                await _reminderService.ScheduleReminderAsync(routine);
            else
                _reminderService.CancelReminder(routine.Id);
            await LoadAsync();
        }

        [RelayCommand]
        private async Task DeleteRoutine(Routine routine)
        {
            _reminderService.CancelReminder(routine.Id);
            await _routineService.DeleteRoutineAsync(routine.Id, DateTime.Today);
            await LoadAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Planner.Models;
using Planner.Services;

namespace Planner.ViewModels
{
    public partial class CalendarViewModel : ObservableObject
    {
        private readonly RoutineService _routineService;

        [ObservableProperty]
        private Dictionary<string, RoutineDayStatus> _dayStatuses = new();

        [ObservableProperty]
        private DateTime _selectedDate = DateTime.Today;

        [ObservableProperty]
        private List<DateTime> _days = new();

        [ObservableProperty]
        private List<Routine> _routines = new();

        [ObservableProperty]
        private string _monthTitle = string.Empty;

        [ObservableProperty]
        private string _completionSummary = string.Empty;

        public CalendarViewModel(RoutineService routineService)
        {
            _routineService = routineService;
            GenerateDays();
        }

        public async Task LoadAsync()
        {
            await LoadRoutinesAsync();
            await LoadDayStatusesAsync();
        }

        partial void OnSelectedDateChanged(DateTime value)
        {
            GenerateDays();
            _ = LoadRoutinesAsync();
        }

        private void GenerateDays()
        {
            var firstOfMonth = new DateTime(SelectedDate.Year, SelectedDate.Month, 1);
            var firstDay = firstOfMonth.AddDays(-(int)firstOfMonth.DayOfWeek);
            var list = new List<DateTime>();
            for (int i = 0; i < 35; i++)
            {
                list.Add(firstDay.AddDays(i));
            }
            Days = list;
            MonthTitle = SelectedDate.ToString("MMMM yyyy");
            _ = LoadDayStatusesAsync();
        }

        private async Task LoadDayStatusesAsync()
        {
            DayStatuses = await GetStatusesForDays(Days);
        }

        private async Task<Dictionary<string, RoutineDayStatus>> GetStatusesForDays(IEnumerable<DateTime> dates)
        {
            var dict = new Dictionary<string, RoutineDayStatus>();
            var routines = await _routineService.GetRoutinesForDates(dates);
            foreach (var date in dates)
            {
                if (routines.TryGetValue(date, out var list) && list.Count > 0)
                {
                    dict[date.ToString("yyyy-MM-dd")] = list.All(r => r.IsCompleted)
                        ? RoutineDayStatus.Complete
                        : RoutineDayStatus.Partial;
                }
                else
                {
                    dict[date.ToString("yyyy-MM-dd")] = RoutineDayStatus.None;
                }
            }
            return dict;
        }

        private async Task LoadRoutinesAsync()
        {
            Routines = await _routineService.GetRoutinesByDate(SelectedDate);
            if (Routines.Count > 0)
                CompletionSummary = $"{Routines.Count(r => r.IsCompleted)}/{Routines.Count} done today";
            else
                CompletionSummary = string.Empty;
            DayStatuses[SelectedDate.ToString("yyyy-MM-dd")] = Routines.Count == 0 ? RoutineDayStatus.None : (Routines.All(r => r.IsCompleted) ? RoutineDayStatus.Complete : RoutineDayStatus.Partial);
            OnPropertyChanged(nameof(DayStatuses));
        }

        [RelayCommand]
        private void SelectDate(DateTime date)
        {
            SelectedDate = date;
        }

        [RelayCommand]
        private async Task ToggleRoutine(Routine routine)
        {
            routine.IsCompleted = !routine.IsCompleted;
            routine.Date = SelectedDate;
            await _routineService.SaveRoutineForDate(routine);
            await LoadRoutinesAsync();
        }

        [RelayCommand]
        private async Task AddRoutine()
        {
            var name = await Shell.Current.DisplayPromptAsync("New Routine", "Name of the routine?");
            if (string.IsNullOrWhiteSpace(name))
                return;
            var routine = new Routine { Name = name, Date = SelectedDate };
            await _routineService.SaveRoutineForDate(routine);
            await LoadRoutinesAsync();
        }

        [RelayCommand]
        private async Task EditRoutine(Routine routine)
        {
            var name = await Shell.Current.DisplayPromptAsync("Edit Routine", "Routine name", initialValue: routine.Name);
            if (string.IsNullOrWhiteSpace(name))
                return;
            routine.Name = name;
            routine.Date = SelectedDate;
            await _routineService.SaveRoutineForDate(routine);
            await LoadRoutinesAsync();
        }

        [RelayCommand]
        private async Task DeleteRoutine(Routine routine)
        {
            await _routineService.DeleteRoutineAsync(routine.Id, SelectedDate);
            await LoadRoutinesAsync();
        }

        public RoutineDayStatus GetCompletionStatus(DateTime date)
        {
            var key = date.ToString("yyyy-MM-dd");
            if (DayStatuses.TryGetValue(key, out var status))
                return status;
            return RoutineDayStatus.None;
        }
    }
}

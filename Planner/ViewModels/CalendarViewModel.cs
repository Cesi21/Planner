using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Planner.Models;
using Planner.Services;

namespace Planner.ViewModels
{
    public partial class CalendarViewModel : ObservableObject
    {
        private readonly RoutineService _routineService;

        [ObservableProperty]
        private DateTime _selectedDate = DateTime.Today;

        [ObservableProperty]
        private List<DateTime> _days = new();

        [ObservableProperty]
        private List<Routine> _routines = new();

        [ObservableProperty]
        private string _monthTitle = string.Empty;

        public CalendarViewModel(RoutineService routineService)
        {
            _routineService = routineService;
            GenerateDays();
        }

        public async Task LoadAsync()
        {
            await LoadRoutinesAsync();
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
        }

        private async Task LoadRoutinesAsync()
        {
            Routines = await _routineService.GetRoutinesByDate(SelectedDate);
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
            var routine = new Routine { Name = "New Routine", Date = SelectedDate };
            await _routineService.SaveRoutineForDate(routine);
            await LoadRoutinesAsync();
        }
    }
}

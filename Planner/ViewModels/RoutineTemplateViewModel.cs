using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Planner.Models;
using Planner.Services;

namespace Planner.ViewModels
{
    public partial class RoutineTemplateViewModel : ObservableObject
    {
        private readonly RoutineService _routineService;

        [ObservableProperty]
        private List<Routine> _templates = new();

        public RoutineTemplateViewModel(RoutineService routineService)
        {
            _routineService = routineService;
        }

        public async Task LoadAsync()
        {
            Templates = await _routineService.GetTemplatesAsync();
        }

        [RelayCommand]
        private async Task AddTemplate()
        {
            var name = await Shell.Current.DisplayPromptAsync("New Template", "Routine name?");
            if (string.IsNullOrWhiteSpace(name))
                return;
            var days = await PromptForDaysAsync(new List<DayOfWeek>());
            var template = new Routine { Name = name, IsTemplate = true, RepeatOnDays = days };
            await _routineService.AddOrUpdateTemplateAsync(template);
            await LoadAsync();
        }

        [RelayCommand]
        private async Task EditTemplate(Routine template)
        {
            var name = await Shell.Current.DisplayPromptAsync("Edit Template", "Routine name", initialValue: template.Name);
            if (string.IsNullOrWhiteSpace(name))
                return;
            var days = await PromptForDaysAsync(template.RepeatOnDays);
            template.Name = name;
            template.RepeatOnDays = days;
            await _routineService.AddOrUpdateTemplateAsync(template);
            await LoadAsync();
        }

        [RelayCommand]
        private async Task DeleteTemplate(Routine template)
        {
            await _routineService.DeleteTemplateAsync(template.Id);
            await LoadAsync();
        }

        private async Task<List<DayOfWeek>> PromptForDaysAsync(IList<DayOfWeek> existing)
        {
            var result = new List<DayOfWeek>(existing);
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                bool include = existing.Contains(day);
                var yes = await Shell.Current.DisplayAlert("Repeat Days", $"Include {day}?", "Yes", "No");
                if (yes)
                {
                    if (!result.Contains(day))
                        result.Add(day);
                }
                else
                {
                    result.Remove(day);
                }
            }
            return result;
        }
    }
}


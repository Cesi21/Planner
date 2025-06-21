using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Planner.Models;
using Planner.Services;

namespace Planner.ViewModels
{
    public partial class GoalListViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        [ObservableProperty]
        private List<Goal> _goals = new();

        public GoalListViewModel(DataService dataService)
        {
            _dataService = dataService;
        }

        public async Task LoadAsync()
        {
            Goals = await _dataService.GetGoalsAsync();
        }

        [RelayCommand]
        private async Task AddGoal()
        {
            var goal = new Goal { Title = "New Goal", TargetValue = 1 };
            await _dataService.AddGoalAsync(goal);
            await LoadAsync();
        }

        [RelayCommand]
        private async Task IncrementGoal(Goal goal)
        {
            if (goal.IsCompleted)
                return;

            goal.CurrentValue++;
            if (goal.CurrentValue >= goal.TargetValue)
            {
                goal.IsCompleted = true;
            }

            await _dataService.UpdateGoalAsync(goal);
            await LoadAsync();
        }
    }
}

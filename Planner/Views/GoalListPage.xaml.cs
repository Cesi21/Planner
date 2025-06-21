using System;
using Planner.ViewModels;

namespace Planner.Views
{
    public partial class GoalListPage : ContentPage
    {
        private readonly GoalListViewModel _vm;
        public GoalListPage(GoalListViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = _vm;
            Loaded += GoalListPage_Loaded;
        }

        private async void GoalListPage_Loaded(object? sender, EventArgs e)
        {
            await _vm.LoadAsync();
        }

        private void OnIncrementGoal(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Planner.Models.Goal goal)
            {
                _vm.IncrementGoalCommand.Execute(goal);
            }
        }
    }
}

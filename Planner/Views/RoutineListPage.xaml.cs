using System;
using Planner.ViewModels;

namespace Planner.Views
{
    public partial class RoutineListPage : ContentPage
    {
        private readonly RoutineListViewModel _vm;
        public RoutineListPage(RoutineListViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = _vm;
            Loaded += RoutineListPage_Loaded;
        }

        private async void RoutineListPage_Loaded(object? sender, EventArgs e)
        {
            await _vm.LoadAsync();
        }

        private void OnRoutineChecked(object? sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox cb && cb.BindingContext is Planner.Models.Routine routine)
            {
                _vm.ToggleRoutineCommand.Execute(routine);
            }
        }
    }
}

using System;
using Planner.ViewModels;

namespace Planner.Views
{
    public partial class CalendarPage : ContentPage
    {
        private readonly CalendarViewModel _vm;
        public CalendarPage(CalendarViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = _vm;
            Loaded += CalendarPage_Loaded;
        }

        private async void CalendarPage_Loaded(object? sender, EventArgs e)
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

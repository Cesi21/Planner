using System;
using Planner.ViewModels;

namespace Planner.Views
{
    public partial class StatsPage : ContentPage
    {
        private readonly StatsViewModel _vm;

        public StatsPage(StatsViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = _vm;
            Loaded += StatsPage_Loaded;
        }

        private async void StatsPage_Loaded(object? sender, EventArgs e)
        {
            await _vm.LoadAsync();
        }
    }
}

using System;
using Microsoft.Maui.Controls;
using Planner.ViewModels;

namespace Planner.Views
{
    public partial class RoutineTemplatePage : ContentPage
    {
        private readonly RoutineTemplateViewModel _vm;
        public RoutineTemplatePage(RoutineTemplateViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = _vm;
            Loaded += RoutineTemplatePage_Loaded;
        }

        private async void RoutineTemplatePage_Loaded(object? sender, EventArgs e)
        {
            await _vm.LoadAsync();
        }
    }
}


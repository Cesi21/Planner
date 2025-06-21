using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Planner.Models;
using Planner.ViewModels;

namespace Planner.Converters
{
    public class CompletionColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date && parameter is Element element && element.BindingContext is CalendarViewModel vm)
            {
                var status = vm.GetCompletionStatus(date);
                return status switch
                {
                    RoutineDayStatus.Complete => Colors.Green,
                    RoutineDayStatus.Partial => Colors.Red,
                    _ => Colors.Transparent
                };
            }
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}

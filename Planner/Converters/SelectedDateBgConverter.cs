using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Planner.ViewModels;

namespace Planner.Converters
{
    public class SelectedDateBgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date && parameter is Element element && element.BindingContext is CalendarViewModel vm)
            {
                return vm.SelectedDate.Date == date.Date ? Colors.LightBlue : Colors.Transparent;
            }
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

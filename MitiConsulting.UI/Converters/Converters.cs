using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace MitiConsulting.UI.Converters
{
    public class PageButtonBackgroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isCurrentPage)
            {
                return isCurrentPage ? Brushes.LightBlue : Brushes.Transparent;
            }
            return Brushes.Transparent;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PageButtonForegroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isCurrentPage)
            {
                return isCurrentPage ? Brushes.Black : Brushes.Blue;
            }
            return Brushes.Blue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsPageNumberEnabledConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int number)
            {
                return number > 0; // Les boutons avec des points de suspension (négatifs) sont désactivés
            }
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
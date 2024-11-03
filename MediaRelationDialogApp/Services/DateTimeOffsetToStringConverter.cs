using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MediaRelationDialogApp.Services
{
    public class DateTimeOffsetToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.ToString("dd-MM-yyyy");
            }
            return string.Empty; // Returner en tom streng, hvis værdien ikke er en DateTimeOffset
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Vi har ikke brug for ConvertBack i dette tilfælde
        }
    }
}

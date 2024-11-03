using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MediaRelationDialogApp.Services
{
    public class SelectedItemToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                // Vi tjekker, om det bindede element er det valgte.
                return Colors.LightBlue;
            }
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;

namespace SendArchive
{
    public class ConverterTabEnumToTabIndex : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is TabMailWindow)
            {
                return (int)value;
            }
            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (TabMailWindow)value;
        }
    }
}
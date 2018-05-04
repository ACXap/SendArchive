using System;
using System.Globalization;
using System.Windows.Data;

namespace SendArchive
{
    /// <summary>
    /// Class converter size file to KB
    /// </summary>
    public class ConverterSizeFileToKb : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var size = (long)value;
                double sizeToKb = Math.Ceiling(size / 1024.0);
                return sizeToKb;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
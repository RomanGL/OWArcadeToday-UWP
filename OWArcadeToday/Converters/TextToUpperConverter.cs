using System;
using Windows.UI.Xaml.Data;

namespace OWArcadeToday.Converters
{
    public sealed class TextToUpperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string str = value?.ToString();
            return str?.ToUpperInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

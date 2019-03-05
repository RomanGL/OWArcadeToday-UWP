using System;
using Windows.UI.Xaml.Data;
using Humanizer;

namespace OWArcadeToday.Converters
{
    public sealed class DateTimeToRelativeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = (DateTime)value;
            return date.Humanize();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

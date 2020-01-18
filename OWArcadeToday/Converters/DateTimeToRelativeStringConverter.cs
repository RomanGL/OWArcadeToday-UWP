using System;
using Windows.UI.Xaml.Data;
using Humanizer;

namespace OWArcadeToday.Converters
{
    /// <summary>
    /// Represents a <seealso cref="DateTime"/> to relative string converter.
    /// </summary>
    public sealed class DateTimeToRelativeStringConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = (DateTime)value;
            return date.Humanize();
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();

        #endregion
    }
}

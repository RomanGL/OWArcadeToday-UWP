using OWArcadeToday.Core.Models;
using System;
using Windows.UI.Xaml.Data;

namespace OWArcadeToday.Converters
{
    /// <summary>
    /// Represents a <seealso cref="GameModeChangesType"/> to string converter.
    /// </summary>
    public sealed class GameModeChangesTypeToChangesTextConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is GameModeChangesType changes)
            {
                switch (changes)
                {
                    case GameModeChangesType.Daily:
                        return "Changes daily";

                    case GameModeChangesType.Weekly:
                        return "Changes weekly";

                    default:
                        return null;
                }
            }

            return null;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
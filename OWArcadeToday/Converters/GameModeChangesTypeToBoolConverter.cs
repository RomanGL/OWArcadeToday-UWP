using OWArcadeToday.Core.Models;
using System;
using Windows.UI.Xaml.Data;

namespace OWArcadeToday.Converters
{
    /// <summary>
    /// Represents a <seealso cref="GameModeChangesType"/> to boolean converter.
    /// </summary>
    public sealed class GameModeChangesTypeToBoolConverter : IValueConverter
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
                    case GameModeChangesType.Weekly:
                        return true;

                    default:
                        return false;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
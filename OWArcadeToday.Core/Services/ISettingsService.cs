namespace OWArcadeToday.Core.Services
{
    /// <summary>
    /// Represents an application settings storage service.
    /// </summary>
    public interface ISettingsService
    {
        #region Methods

        /// <summary>
        /// Clears the settings storage.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes the parameter value from the settings storage.
        /// </summary>
        /// <param name="parameterName">The parameter name in the settings storage.</param>
        void Remove(string parameterName);

        /// <summary>
        /// Gets the value indicating whether the parameter exists in the settings storage.
        /// </summary>
        /// <param name="parameterName">The parameter name in the settings storage.</param>
        /// <returns>Returns <c>true</c> if the parameter exists, otherwise <c>false</c>.</returns>
        bool Contains(string parameterName);

        /// <summary>
        /// Write the parameter value to the settings storage.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameterName">The parameter name in the settings storage.</param>
        /// <param name="value">Th value.</param>
        void Set<T>(string parameterName, T value);

        /// <summary>
        /// Gets the parameter value from the settings storage.
        /// If value cached, will returned value from the cache.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameterName">The parameter name in the settings storage.</param>
        /// <param name="defaultValue">
        /// If no value exists, will returned this value, and this value will be wrote to the settings storage.
        /// </param>
        /// <returns>
        /// Returns the parameter value from the settings storage or cache. If value is not exists returns <paramref name="defaultValue"/>.
        /// </returns>
        T Get<T>(string parameterName, T defaultValue = default(T));

        /// <summary>
        /// Gets the parameter value directly from the storage server.
        /// A cached value is not used here.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameterName">The parameter name in the settings storage.</param>
        /// <param name="defaultValue">
        /// If no value exists, will returned this value, and this value will be wrote to the settings storage.
        /// </param>
        /// <returns>
        /// Returns the parameter value from the settings storage or cache. If value is not exists returns <paramref name="defaultValue"/>.
        /// </returns>
        T GetNoCache<T>(string parameterName, T defaultValue = default(T));

        #endregion
    }
}
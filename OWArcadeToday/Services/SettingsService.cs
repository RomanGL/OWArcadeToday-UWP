using Newtonsoft.Json;
using OWArcadeToday.Core.Services;
using System.Collections.Generic;
using Windows.Storage;

namespace OWArcadeToday.Services
{
    /// <inheritdoc />
    internal sealed class SettingsService : ISettingsService
    {
        #region Fields

        private readonly Dictionary<string, object> cache = new Dictionary<string, object>();
        private readonly object lockObject = new object();

        #endregion

        #region Implementation of ISettingsService

        /// <inheritdoc />
        public void Clear()
        {
            lock (lockObject)
            {
                ApplicationData.Current.LocalSettings.Values.Clear();
                cache.Clear();
            }
        }

        /// <inheritdoc />
        public void Remove(string parameterName)
        {
            lock (lockObject)
            {
                cache.Remove(parameterName);
                ApplicationData.Current.LocalSettings.Values.Remove(parameterName);
            }
        }

        /// <inheritdoc />
        public bool Contains(string parameterName)
            => ApplicationData.Current.LocalSettings.Values.ContainsKey(parameterName);

        /// <inheritdoc />
        public void Set<T>(string parameterName, T value)
        {
            var json = JsonConvert.SerializeObject(value);

            lock (lockObject)
            {
                ApplicationData.Current.LocalSettings.Values[parameterName] = json;
                cache[parameterName] = value;
            }
        }

        /// <inheritdoc />
        public T Get<T>(string parameterName, T defaultValue = default(T))
        {
            lock (lockObject)
            {
                cache.TryGetValue(parameterName, out var readed);

                if (readed != null)
                    return (T) readed;

                ApplicationData.Current.LocalSettings.Values.TryGetValue(parameterName, out readed);
                if (readed != null)
                {
                    var json = readed.ToString();
                    var value = JsonConvert.DeserializeObject<T>(json);

                    cache[parameterName] = value;
                    return value;
                }
                else
                {
                    var json = JsonConvert.SerializeObject(defaultValue);
                    ApplicationData.Current.LocalSettings.Values[parameterName] = json;
                    cache[parameterName] = defaultValue;
                    return defaultValue;
                }
            }
        }

        /// <inheritdoc />
        public T GetNoCache<T>(string parameterName, T defaultValue = default(T))
        {
            lock (lockObject)
            {
                ApplicationData.Current.LocalSettings.Values.TryGetValue(parameterName, out var readed);
                if (readed != null)
                {
                    var json = readed.ToString();
                    var value = JsonConvert.DeserializeObject<T>(json);
                    return value;
                }
                else
                {
                    var json = JsonConvert.SerializeObject(defaultValue);
                    ApplicationData.Current.LocalSettings.Values[parameterName] = json;
                    return defaultValue;
                }
            }
        }

        #endregion
    }
}
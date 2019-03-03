using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.Storage;

namespace BackgroundTasks.Services
{
    internal sealed class SettingsService
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
        private readonly object _lockObject = new object();

        public void Clear()
        {
            lock (_lockObject)
            {
                ApplicationData.Current.LocalSettings.Values.Clear();
                _cache.Clear();
            }
        }

        public void Set<T>(string parameterName, T value)
        {
            string json = JsonConvert.SerializeObject(value);

            lock (_lockObject)
            {
                ApplicationData.Current.LocalSettings.Values[parameterName] = json;
                _cache[parameterName] = value;
            }
        }

        public T Get<T>(string parameterName, T defaultValue = default(T))
        {
            lock (_lockObject)
            {
                object readed;
                _cache.TryGetValue(parameterName, out readed);

                if (readed != null)
                    return (T)readed;

                ApplicationData.Current.LocalSettings.Values.TryGetValue(parameterName, out readed);
                if (readed != null)
                {
                    string json = readed.ToString();
                    T value = JsonConvert.DeserializeObject<T>(json);

                    _cache[parameterName] = value;
                    return value;
                }
                else
                {
                    string json = JsonConvert.SerializeObject(defaultValue);
                    ApplicationData.Current.LocalSettings.Values[parameterName] = json;
                    _cache[parameterName] = defaultValue;
                    return defaultValue;
                }
            }
        }

        public T GetNoCache<T>(string parameterName, T defaultValue = default(T))
        {
            lock (_lockObject)
            {
                object readed;
                ApplicationData.Current.LocalSettings.Values.TryGetValue(parameterName, out readed);
                if (readed != null)
                {
                    string json = readed.ToString();
                    T value = JsonConvert.DeserializeObject<T>(json);
                    return value;
                }
                else
                {
                    string json = JsonConvert.SerializeObject(defaultValue);
                    ApplicationData.Current.LocalSettings.Values[parameterName] = json;
                    return defaultValue;
                }
            }
        }

        public void Remove(string parameterName)
        {
            lock (_lockObject)
            {
                if (_cache.ContainsKey(parameterName))
                    _cache.Remove(parameterName);
                if (ContainsSetting(parameterName))
                    ApplicationData.Current.LocalSettings.Values.Remove(parameterName);
            }
        }

        public bool ContainsSetting(string parameterName)
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey(parameterName);
        }
    }
}

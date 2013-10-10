using System.IO.IsolatedStorage;

namespace PhoneCoreLibrary.Storage
{
    /// <summary>
    /// Isolated storage setting service
    /// </summary>
    public class IsolatedStorageSettingService : ISettingService
    {
        private readonly IsolatedStorageSettings _settings;

        public IsolatedStorageSettingService()
        {
            _settings = IsolatedStorageSettings.ApplicationSettings;
        }

        public void Save(string key, object value)
        {
            _settings[key] = value;
            _settings.Save();
        }

        public bool IsExist(string key)
        {
            return _settings.Contains(key);
        }

        public T Load<T>(string key)
        {
            if (!_settings.Contains(key))
                return default(T);

            return (T)_settings[key];
        }
        
        public void Clear()
        {
            _settings.Clear();
        }
    }
}

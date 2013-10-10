namespace PhoneCoreLibrary.Storage
{
    /// <summary>
    /// Provides the access to settings storage
    /// </summary>
    public interface ISettingService
    {
        /// <summary>
        /// Saves the setting
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Save(string key, object value);

        /// <summary>
        /// Checks existance of the setting
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsExist(string key);

        /// <summary>
        /// Loads the setting from storage by key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Load<T>(string key);

        /// <summary>
        /// Clear all settings
        /// </summary>
        void Clear();
    }
}

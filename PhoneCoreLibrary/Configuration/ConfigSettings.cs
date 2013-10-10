using System.Collections.Generic;
using System.Xml.Linq;

namespace PhoneCoreLibrary.Configuration
{
    /// <summary>
    /// Provides the access to the configuration subsystem
    /// </summary>
    public class ConfigSettings
    {
        public const string AppConfigFileName = "application.config";
        private ConfigElement _root;

        private ConfigSettings()
        {
            //load application config
            _root = new ConfigElement(XDocument.Load(AppConfigFileName).Root);
        }
        
        #region singleton
        private static object _syncLock = new object();
        private volatile static ConfigSettings _instance;
        public static ConfigSettings Instance
        {
            get
            {
                if (_instance == null)
                    lock (_syncLock)
                        if (_instance == null)
                            _instance = new ConfigSettings();
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// Get section
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public IConfigSection GetSection(string xpath)
        {
            return (new ConfigSection(_root)).GetSection(xpath);
        }

        /// <summary>
        /// Get the set of sections
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public IEnumerable<IConfigSection> GetSections(string xpath)
        {
            return (new ConfigSection(_root)).GetSections(xpath);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace PhoneCoreLibrary.Configuration
{
    /// <summary>
    /// Represens a config entry
    /// </summary>
    public class ConfigSection : IConfigSection
    {
        private ConfigElement _element;
        public ConfigSection(ConfigElement element)
        {
            _element = element;
        }

        /// <summary>
        /// Returns the set of ConfigSections
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public IEnumerable<IConfigSection> GetSections(string xpath)
        {
            return _element.GetElements(xpath).Select(e => (new ConfigSection(e)) as IConfigSection);
        }

        /// <summary>
        /// Returns ConfigSection
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public IConfigSection GetSection(string xpath)
        {
            return new ConfigSection(new ConfigElement(_element.Node, xpath));
        }

        public bool IsEmpty
        {
            get { return _element.IsEmpty; }
        }

        /// <summary>
        /// Returns string
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public string GetString(string xpath)
        {
            return new ConfigElement(_element.Node, xpath).GetString();
        }

        /// <summary>
        /// Returns int
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public int GetInt(string xpath)
        {
            return new ConfigElement(_element.Node, xpath).GetInt();
        }

        /// <summary>
        /// Returns int
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetInt(string xpath, int defaultValue)
        {
            try
            {
                return GetInt(xpath);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns bool
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public bool GetBool(string xpath)
        {
            return new ConfigElement(_element.Node, xpath).GetBool();
        }

        public bool GetBool(string xpath, bool defaultValue)
        {
            try
            {
                return GetBool(xpath);
            }
            catch
            {
                return defaultValue;
            }
        }
       
        /// <summary>
        /// Returns type object
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public Type GetType(string xpath)
        {
            return (new ConfigElement(_element.Node, xpath)).GetType();
        }


        /// <summary>
        /// Returns the instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public T GetInstance<T>(string xpath)
        {
            return (T)Activator.CreateInstance(GetType(xpath));
        }

        /// <summary>
        /// Returns the instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xpath"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T GetInstance<T>(string xpath, params object[] args)
        {
            return (T)Activator.CreateInstance(GetType(xpath), args);
        }

    }
}

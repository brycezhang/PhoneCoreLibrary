using System;
using Newtonsoft.Json.Linq;

namespace PhoneCoreLibrary.Utils
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public static class JsonUtility
    {
        public static string GetElementValue(JToken element,
            string elementName,
            string defaultReturnValue)
        {
            if (element == null)
                throw new ArgumentNullException("element", "element is null.");
            if (String.IsNullOrEmpty(elementName))
                throw new ArgumentException("elementName is null or empty.", "elementName");

            return element[elementName] == null ? defaultReturnValue : element[elementName].ToString();
        }

        public static DateTime GetElementValue(JToken element,
            string elementName,
            DateTime defaultReturnValue)
        {
            if (element == null)
                throw new ArgumentNullException("element", "element is null.");
            if (String.IsNullOrEmpty(elementName))
                throw new ArgumentException("elementName is null or empty.", "elementName");

            var childElement = element[elementName];

            if (childElement == null)
                return defaultReturnValue;

            DateTime returnValue;

            return DateTime.TryParse(childElement.ToString(), out returnValue) ? returnValue : defaultReturnValue;

        }

        public static int GetElementValue(JToken element,
            string elementName,
            int defaultReturnValue)
        {
            if (element == null)
                throw new ArgumentNullException("element", "element is null.");
            if (String.IsNullOrEmpty(elementName))
                throw new ArgumentException("elementName is null or empty.", "elementName");

            var childElement = element[elementName];

            if (childElement == null)
                return defaultReturnValue;

            int returnValue;
            return Int32.TryParse(childElement.ToString(), out returnValue) ? returnValue : defaultReturnValue;
        }

        public static double GetElementValue(JToken element,
            string elementName,
            double defaultReturnValue)
        {
            if (element == null)
                throw new ArgumentNullException("element", "element is null.");
            if (String.IsNullOrEmpty(elementName))
                throw new ArgumentException("elementName is null or empty.", "elementName");

            var childElement = element[elementName];

            if (childElement == null)
                return defaultReturnValue;

            double returnValue;
            return Double.TryParse(childElement.ToString(), out returnValue) ? returnValue : defaultReturnValue;
        }

        public static JArray GetElementValue(JToken element,
            string elementName,
            JArray defaultReturnValue)
        {
            if (element == null)
                throw new ArgumentNullException("element", "element is null.");
            if (String.IsNullOrEmpty(elementName))
                throw new ArgumentException("elementName is null or empty.", "elementName");

            var childElement = element[elementName] as JArray;

            return childElement ?? defaultReturnValue;
        }
    }
}

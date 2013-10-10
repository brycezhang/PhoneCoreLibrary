using System;
using System.Linq;
using System.Xml.Linq;

namespace PhoneCoreLibrary.Utils
{
    /// <summary>
    /// Xml∞Ô÷˙¿‡
    /// </summary>
    public static class XmlUtility
    {
        public static int GetAttributeValueAsInt32(XElement element, string attributeName, int defaultReturnValue)
        {
            var valueAsString = GetAttributeValue(element, attributeName, defaultReturnValue.ToString());

            int valueAsInt32;

            return Int32.TryParse(valueAsString, out valueAsInt32) ? valueAsInt32 : defaultReturnValue;
        }

        public static string GetAttributeValue(XElement element, string attributeName, string defaultReturnValue)
        {
            if (element == null)
                return defaultReturnValue;

            var value = element.Attribute(attributeName);
            return value == null ? defaultReturnValue : value.Value;
        }

        public static void SetChildElement(XElement parentElement, 
            string childElementName, 
            double childElementValue)
        {
            SetChildElement(parentElement, childElementName, 
                childElementValue.ToString());
        }

        public static void SetChildElement(XElement parentElement,
            string childElementName,
            DateTime childElementValue)
        {
            SetChildElement(parentElement, childElementName,
                childElementValue.ToString());
        }

        public static void SetChildElement(XElement parentElement, string childElementName, string childElementValue)
        {
            if (parentElement == null)
                throw new ArgumentNullException("parentElement", "parentElement is null.");
            if (String.IsNullOrEmpty(childElementName))
                throw new ArgumentException("childElementName is null or empty.", "childElementName");
            if (String.IsNullOrEmpty(childElementValue))
            {
                childElementValue = String.Empty;
            }

            parentElement.SetElementValue(childElementName, childElementValue);
        }

        public static XElement GetChildElement(XElement parentElement, string childElementName)
        {
            if (parentElement == null)
                throw new ArgumentNullException("parentElement", "parentElement is null.");
            if (String.IsNullOrEmpty(childElementName))
                throw new ArgumentException("childElementName is null or empty.", "childElementName");

            var childElement = parentElement.Elements(childElementName).FirstOrDefault();
            
            return childElement;
        }

        public static string GetChildElementValue(XElement parentElement,
            string childElementName,
            string defaultReturnValue)
        {
            if (parentElement == null)
                throw new ArgumentNullException("parentElement", "parentElement is null.");
            if (String.IsNullOrEmpty(childElementName))
                throw new ArgumentException("childElementName is null or empty.", "childElementName");

            XElement childElement = GetChildElement(parentElement, childElementName);

            return childElement == null ? defaultReturnValue : childElement.Value;
        }

        public static DateTime GetChildElementValue(XElement parentElement,
            string childElementName,
            DateTime defaultReturnValue)
        {
            if (parentElement == null)
                throw new ArgumentNullException("parentElement", "parentElement is null.");
            if (String.IsNullOrEmpty(childElementName))
                throw new ArgumentException("childElementName is null or empty.", "childElementName");

            XElement childElement = GetChildElement(parentElement, childElementName);

            if (childElement == null)
                return defaultReturnValue;
            else
            {
                DateTime returnValue;

                return DateTime.TryParse(childElement.Value, out returnValue) ? returnValue : defaultReturnValue;
            }
        }

        public static int GetChildElementValue(XElement parentElement,
            string childElementName,
            int defaultReturnValue)
        {
            if (parentElement == null)
                throw new ArgumentNullException("parentElement", "parentElement is null.");
            if (String.IsNullOrEmpty(childElementName))
                throw new ArgumentException("childElementName is null or empty.", "childElementName");

            XElement childElement = GetChildElement(parentElement, childElementName);

            if (childElement == null)
                return defaultReturnValue;

            int returnValue;
            return Int32.TryParse(childElement.Value, out returnValue) ? returnValue : defaultReturnValue;
        }

        public static string GetChildElementValue(XElement parentElement,
            string childElementName,
            string childAttributeName,
            string childAttributeValue,
            string defaultReturnValue)
        {
            if (parentElement == null)
                throw new ArgumentNullException("parentElement", "parentElement is null.");
            if (String.IsNullOrEmpty(childElementName))
                throw new ArgumentException("childElementName is null or empty.", "childElementName");

            var childElement = (
                from temp in parentElement.Descendants(childElementName)
                where temp.HasAttributes &&
                temp.Attribute(childAttributeName) != null &&
                temp.Attribute(childAttributeName).Value == childAttributeValue
                select temp).FirstOrDefault();

            if (childElement == null)
                return defaultReturnValue;

            return childElement.Value;
        }

        public static XDocument StringToXDocument(string fromValue)
        {
            try
            {
               return XDocument.Parse(fromValue);
            }
            catch
            {
                return null;
            }
        }        
    }
}

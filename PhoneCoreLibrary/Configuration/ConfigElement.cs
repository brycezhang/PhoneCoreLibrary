using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PhoneCoreLibrary.Configuration
{
    /// <summary>
    /// Represens a single element of xml
    /// </summary>
    public class ConfigElement
    {
        private string _xpath;
        private XElement _node;
        private XAttribute _attribute;

        public ConfigElement(XElement root)
        {
            _node = root;
        }

        public ConfigElement(XElement root, string xpath)
        {
            _node = root;
            _xpath = xpath;
            Initialize();
        }

        private void Initialize()
        {
            string[] paths = _xpath.Split('/');

            XElement current = _node;
            for (int i = 0; i < paths.Length; i++)
            {
                if (paths[i].StartsWith("@"))
                {
                    _attribute = current.Attribute(paths[i].Substring(1));
                    return;
                }

                current = current.Element(paths[i]);
                if (current == null)
                    break;
            }

            _node = current;
        }

        /// <summary>
        /// Returns the set of elements
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public IEnumerable<ConfigElement> GetElements(string xpath)
        {
            if (Node == null)
                return Enumerable.Empty<ConfigElement>();

            string[] paths = xpath.Split('/');
            int last = paths.Length - 1;
            XElement current = Node;
            for (int i = 0; i < last; i++)
            {
                current = current.Element(paths[i]);
                //xpath isn't valid
                if (current == null)
                    return Enumerable.Empty<ConfigElement>();
            }

            return current.Elements(paths[last]).Select(x => new ConfigElement(x));
        }

        /// <summary>
        /// Returns string
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            if (IsAttribute) return _attribute.Value;
            if (IsNode) return _node.Value;

            return null;
        }

        /// <summary>
        /// Returns int
        /// </summary>
        /// <returns></returns>
        public int GetInt()
        {
            return int.Parse(GetString());
        }

        /// <summary>
        /// Returns boolean
        /// </summary>
        /// <returns></returns>
        public bool GetBool()
        {
            return bool.Parse(GetString());
        }

        /// <summary>
        /// Returns type
        /// </summary>
        /// <returns></returns>
        public new Type GetType()
        {
            string typeName = GetString();
            return Type.GetType(typeName);
        }

        /// <summary>
        /// Returns current XElement
        /// </summary>
        public XElement Node
        {
            get { return _node; }
        }

        /// <summary>
        /// Returns current XAttribute
        /// </summary>
        public XAttribute Attribute
        {
            get { return _attribute; }
        }

        /// <summary>
        /// true if element represents attribute
        /// </summary>
        public bool IsAttribute
        {
            get { return _attribute != null; }
        }

        /// <summary>
        /// true if element represents xml node
        /// </summary>
        public bool IsNode
        {
            get { return _node != null; }
        }

        public bool IsEmpty
        {
            get
            {
                 return (!IsAttribute) && (!IsNode);
            }
        }

    }
}

using System;
using System.Configuration;

namespace SynoDuplicateFolders.Configuration
{
    public class NamedBasicConfigurationElementMap<T> : ConfigurationElement where T : ConfigurationElement, IElementProvider, new()
    {
        [ConfigurationProperty("", IsRequired = true, IsKey = true, IsDefaultCollection = true)]
        public BasicConfigurationElementMap<T> Items
        {
            get { return ((BasicConfigurationElementMap<T>)(base[""])); }
            set { base[""] = value; }
        }
        [ConfigurationProperty("name")]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
            set
            {
                this["name"] = value;
            }
        }

    }

    public class BasicConfigurationElementMap<T> : ConfigurationElementCollection where T : ConfigurationElement, IElementProvider, new()
    {
        private readonly IElementProvider _provider;
        public BasicConfigurationElementMap()
            : base()
        {

            _provider = new T() as IElementProvider;

            string capital1 = _provider.GetElementName();
            this.AddElementName = capital1;

            capital1 = capital1.Substring(0, 1).ToUpper() + capital1.Substring(1);

            this.ClearElementName = "clear" + capital1;
            this.RemoveElementName = "remove" + capital1;

        }

        public string GetElementName()
        {
            return _provider.GetElementName();
        }

        public T this[object key]
        {
            get
            {
                return base.BaseGet(key) as T;
            }

        }
        public bool ContainsKey(object key)
        {
            return this[(object)key] != null;
        }
        public T this[int index]
        {
            get
            {
                return base.BaseGet(index) as T;
            }
        }

        public void Add(T element)
        {
            BaseAdd(element, true);
        }

        public void Remove(T element)
        {
            BaseRemove(element.GetElementKey());
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public int IndexOf(T element)
        {
            return BaseIndexOf(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return _provider.GetElementName();
            }
        }
        protected override bool IsElementName(string elementName)
        {
            bool isName = false;
            string name = this.ElementName;
            if (!String.IsNullOrEmpty(name))
                isName = elementName.Equals(name);
            return isName;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            IElementProvider k = element as IElementProvider;
            return k.GetElementKey();
        }

    }

    public interface IElementProvider
    {
        string GetElementName();
        object GetElementKey();
    }
}

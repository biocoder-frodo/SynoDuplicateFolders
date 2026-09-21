using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;

namespace SynoDuplicateFolders.Data.Core
{
    public static partial class Extensions
    {
        public static T GetDefaultValueAttribute<T>(this object instance, string propertyName)
        {
            var attribute = TypeDescriptor.GetProperties(instance)[propertyName].Attributes.OfType<DefaultValueAttribute>().Single();
            return (T)attribute.Value;
        }
        public static T GetPropertyAttribute<A, T>(this object instance, string propertyName, Func<A, T> getter = null) where A : Attribute
        {
            var attribute = TypeDescriptor.GetProperties(instance)[propertyName].Attributes.OfType<A>().Single();
            return getter is null ? (T)((attribute as DefaultValueAttribute).Value) : getter(attribute);
        }
        public static T GetDefaultValueAttribute<T>(this ConfigurationElement instance, string propertyName)
        {
            return GetPropertyAttribute<ConfigurationPropertyAttribute, T>(instance, propertyName, p => (T)p.DefaultValue);
        }
    }
}

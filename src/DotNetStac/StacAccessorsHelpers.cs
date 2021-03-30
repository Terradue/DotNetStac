using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Stac
{
    public static class StacAccessorsHelpers
    {

        public static void SetProperty(this IStacObject stacObject, string key, object value)
        {
            stacObject.Properties.SetProperty(key, value);
        }

        public static void SetProperty(this IStacPropertiesContainer stacPropertiesContainer, string key, object value)
        {
            stacPropertiesContainer.Properties.SetProperty(key, value);
        }

        public static void SetProperty(this IDictionary<string, object> properties, string key, object value)
        {
            properties.Remove(key);
            properties.Add(key, value);
        }

        public static object GetProperty(this IStacPropertiesContainer propertiesContainer, string key)
        {
            return propertiesContainer.Properties.GetProperty(key);
        }

        public static T GetProperty<T>(this IStacPropertiesContainer propertiesContainer, string key)
        {
            return propertiesContainer.Properties.GetProperty<T>(key);
        }

        public static object GetProperty(this IDictionary<string, object> properties, string key)
        {
            if (!properties.ContainsKey(key))
                return null;
            return properties[key];
        }

        public static T GetProperty<T>(this IDictionary<string, object> properties, string key)
        {
            var @object = GetProperty(properties, key);
            if (@object == null) return default(T);
            if (@object is JToken)
                return (@object as JToken).ToObject<T>();
            if (typeof(T).GetTypeInfo().IsEnum)
                return (T)Enum.Parse(typeof(T), @object.ToString());
            return (T)Convert.ChangeType(@object, typeof(T));
        }

    }
}

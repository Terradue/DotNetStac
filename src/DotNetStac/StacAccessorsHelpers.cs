// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacAccessorsHelpers.cs

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Common;

namespace Stac
{
    /// <summary>
    /// Helper class for accessing properties in Stac objects
    /// </summary>
    public static class StacAccessorsHelpers
    {
        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="stacObject">The stac object.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void SetProperty(this IStacObject stacObject, string key, object value)
        {
            stacObject.Properties.SetProperty(key, value);
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="stacPropertiesContainer">The stac properties container.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void SetProperty(this IStacPropertiesContainer stacPropertiesContainer, string key, object value)
        {
            stacPropertiesContainer.Properties.SetProperty(key, value);
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="properties">The properties dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void SetProperty(this IDictionary<string, object> properties, string key, object value)
        {
            properties.Remove(key);
            properties.Add(key, value);
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="propertiesContainer">The stac properties container.</param>
        /// <param name="key">The key.</param>
        /// <returns>the property value</returns>
        public static object GetProperty(this IStacPropertiesContainer propertiesContainer, string key)
        {
            return propertiesContainer.Properties.GetProperty(key);
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="T">the type of the property</typeparam>
        /// <param name="propertiesContainer">The stac properties container.</param>
        /// <param name="key">The key.</param>
        /// <returns>the property value</returns>
        public static T GetProperty<T>(this IStacPropertiesContainer propertiesContainer, string key)
        {
            return propertiesContainer.Properties.GetProperty<T>(key);
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="properties">The properties dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns>the property value</returns>
        public static object GetProperty(this IDictionary<string, object> properties, string key)
        {
            if (!properties.ContainsKey(key))
            {
                return null;
            }

            return properties[key];
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="T">the type of the property</typeparam>
        /// <param name="properties">The properties dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns>the property value</returns>
        public static T GetProperty<T>(this IDictionary<string, object> properties, string key)
        {
            var @object = GetProperty(properties, key);
            if (@object == null)
            {
                return default(T);
            }

            if (@object is JToken)
            {
                return (@object as JToken).ToObject<T>();
            }

            var t = typeof(T);
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                t = Nullable.GetUnderlyingType(t);
            }

            if (t.GetTypeInfo().IsEnum)
            {
                return (T)LazyEnumParse(t, @object.ToString());
            }

            return ChangeType<T>(@object);
        }

        /// <summary>
        /// Parse an enum value from a string with a fallback to the many attributes that can be used to define an enum value
        /// </summary>
        /// <param name="t">the enum type</param>
        /// <param name="value">the string value</param>
        /// <returns>the enum value</returns>
        public static object LazyEnumParse(Type t, string value)
        {
            // First try with the default Enum.Parse
            try
            {
                return Enum.Parse(t, value);
            }
            catch
            {
            }

            // Then try each enum value
            foreach (object enumValue in Enum.GetValues(t))
            {
                string[] possibleValues = enumValue.ToStringByAttributes();
                if (possibleValues.Contains(value, StringComparer.InvariantCulture))
                {
                    return Enum.Parse(t, enumValue.ToString());
                }
            }

            throw new ArgumentException($"Could not parse {value} to {t.Name}");
        }

        /// <summary>
        /// Get the string value of an enum value
        /// </summary>
        /// <param name="value">the enum value</param>
        /// <returns>the string value</returns>
        public static string[] ToStringByAttributes(this object value)
        {
            List<string> values = new List<string>();

            var field = value
                .GetType()
                .GetField(value.ToString());

            if (field == null)
            {
                return values.ToArray();
            }

            var enumMemberAttribute = GetEnumMemberAttribute(field);
            if (enumMemberAttribute != null)
            {
                values.Add(enumMemberAttribute.Value ?? string.Empty);
            }

            var descriptionAttribute = GetDescriptionAttribute(field);
            if (descriptionAttribute != null)
            {
                values.Add(descriptionAttribute.Description ?? string.Empty);
            }

            var jsonPropertyAttribute = GetJsonPropertyAttribute(field);
            if (jsonPropertyAttribute != null)
            {
                values.Add(jsonPropertyAttribute.PropertyName ?? string.Empty);
            }

            return values.ToArray();
        }

        /// <summary>
        /// Gets the property as an observable collection.
        /// </summary>
        /// <typeparam name="T">the type of the property</typeparam>
        /// <param name="propertiesContainer">The properties container.</param>
        /// <param name="key">The key.</param>
        /// <returns>the property collection</returns>
        public static PropertyObservableCollection<T> GetObservableCollectionProperty<T>(this IStacPropertiesContainer propertiesContainer, string key)
        {
            List<T> array = new List<T>();
            try
            {
                array = propertiesContainer.GetProperty<List<T>>(key);
            }
            catch
            {
                array = propertiesContainer.GetProperty<T[]>(key)?.ToList();
            }

            PropertyObservableCollection<T> observableCollection = new PropertyObservableCollection<T>(propertiesContainer, key);
            if (array != null && array.Count() > 0)
            {
                observableCollection.AddRange(array);
            }

            return observableCollection;
        }

        /// <summary>
        /// Changes the type.
        /// </summary>
        /// <typeparam name="T">the type of the property</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>the changed type value</returns>
        public static T ChangeType<T>(object value)
        {
            var t = typeof(T);

            // if the value is assignable to the type, just return it
            if (t.IsAssignableFrom(value.GetType()))
            {
                return (T)value;
            }

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return (T)Convert.ChangeType(value, t);
        }

        /// <summary>
        /// Removes the property.
        /// </summary>
        /// <param name="propertiesContainer">The stac properties container.</param>
        /// <param name="key">The key.</param>
        public static void RemoveProperty(this IStacPropertiesContainer propertiesContainer, string key)
        {
            propertiesContainer.Properties.RemoveProperty(key);
        }

        /// <summary>
        /// Removes the property.
        /// </summary>
        /// <param name="properties">The properties dictionary.</param>
        /// <param name="key">The key.</param>
        public static void RemoveProperty(this IDictionary<string, object> properties, string key)
        {
            properties.Remove(key);
        }

        /// <summary>
        /// Gets the children links.
        /// </summary>
        /// <param name="stacCatalog">The stac parent.</param>
        /// <returns>the children links</returns>
        public static IEnumerable<StacLink> GetChildrenLinks(this IStacParent stacCatalog)
        {
            return stacCatalog.Links.Where(l => l.RelationshipType == "child");
        }

        /// <summary>
        /// Gets the item links.
        /// </summary>
        /// <param name="stacCatalog">The stac parent.</param>
        /// <returns>the item links</returns>
        public static IEnumerable<StacLink> GetItemLinks(this IStacParent stacCatalog)
        {
            return stacCatalog.Links.Where(l => l.RelationshipType == "item");
        }

        /// <summary>
        /// Set the id of the STAC Collection this Item references to
        /// see <seealso href="https://github.com/radiantearth/stac-spec/blob/master/item-spec/item-spec.md#relation-types">collection relation type</seealso>.
        /// </summary>
        /// <param name="stacItem">stacItem to set the collection to</param>
        /// <param name="collectionId">identifier of the collection</param>
        /// <param name="collectionUri">uri to the collection</param>
        /// <param name="collectionTitle">optional title of the collection</param>
        public static void SetCollection(this StacItem stacItem, string collectionId, Uri collectionUri, string collectionTitle = null)
        {
            var existingLink = stacItem.Links.FirstOrDefault(l => l.Uri == collectionUri);
            if (existingLink != null)
            {
                stacItem.Links.Remove(existingLink);
            }

            stacItem.Links.Add(StacLink.CreateCollectionLink(collectionUri));
            stacItem.Collection = collectionId;
        }

        /// <summary>
        /// Gets the collection of the Item as a StacLink
        /// </summary>
        /// <param name="stacItem">The stac item.</param>
        /// <returns>a Stac Link</returns>
        public static StacLink GetCollection(this StacItem stacItem)
        {
            return stacItem.Links.FirstOrDefault(l => l.RelationshipType == "collection");
        }

        /// <summary>
        /// Adds a range of items to the collection.
        /// </summary>
        /// <typeparam name="T">the type of the collection</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items.</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection is List<T> list)
            {
                list.AddRange(items);
            }
            else
            {
                foreach (T item in items)
                {
                    collection.Add(item);
                }
            }
        }

        /// <summary>
        /// Inserts the item at the specified index in the collection.
        /// </summary>
        /// <typeparam name="T">the type of the collection</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public static void Insert<T>(this ICollection<T> collection, int index, T item)
        {
            if (index < 0 || index > collection.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index was out of range. Must be non-negative and less than the size of the collection.");
            }

            if (collection is IList<T> list)
            {
                list.Insert(index, item);
            }
            else
            {
                List<T> temp = new List<T>(collection);

                collection.Clear();

                collection.AddRange(temp.Take(index));
                collection.Add(item);
                collection.AddRange(temp.Skip(index));
            }
        }

        private static DescriptionAttribute GetDescriptionAttribute(FieldInfo field)
        {
            return field
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .OfType<DescriptionAttribute>()
                .SingleOrDefault();
        }

        private static EnumMemberAttribute GetEnumMemberAttribute(FieldInfo field)
        {
            return field
                .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .OfType<EnumMemberAttribute>()
                .SingleOrDefault();
        }

        private static JsonPropertyAttribute GetJsonPropertyAttribute(FieldInfo field)
        {
            return field
                .GetCustomAttributes(typeof(JsonPropertyAttribute), false)
                .OfType<JsonPropertyAttribute>()
                .SingleOrDefault();
        }
    }
}

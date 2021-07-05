using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Stac.Common;

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
            {
                return (@object as JToken).ToObject<T>();
            }
            if (typeof(T).GetTypeInfo().IsEnum)
                return (T)Enum.Parse(typeof(T), @object.ToString());
            return ChangeType<T>(@object);
        }

        public static PropertyObservableCollection<T> GetObservableCollectionProperty<T>(this IStacPropertiesContainer propertiesContainer, string key)
        {
            List<T> array = propertiesContainer.GetProperty<List<T>>(key);
            PropertyObservableCollection<T> observableCollection = new PropertyObservableCollection<T>(propertiesContainer, key);
            if (array != null && array.Count > 0)
            {
                observableCollection.AddRange<T>(array);
            }
            return observableCollection;
        }



        public static T ChangeType<T>(object value)
        {
            var t = typeof(T);

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

        public static void RemoveProperty(this IStacPropertiesContainer propertiesContainer, string key)
        {
            propertiesContainer.Properties.RemoveProperty(key);
        }

        public static void RemoveProperty(this IDictionary<string, object> properties, string key)
        {
            properties.Remove(key);
        }

        public static IEnumerable<StacLink> GetChildrenLinks(this IStacParent stacCatalog)
        {
            return stacCatalog.Links.Where(l => l.RelationshipType == "child");
        }

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
                stacItem.Links.Remove(existingLink);
            stacItem.Links.Add(StacLink.CreateCollectionLink(collectionUri));
            stacItem.Collection = collectionId;
        }

        /// <summary>
        /// Gets the collection of the Item as a StacLink
        /// </summary>
        /// <param name="stacItem"></param>
        /// <returns>a Stac Link</returns>
        public static StacLink GetCollection(this StacItem stacItem)
        {
            return stacItem.Links.FirstOrDefault(l => l.RelationshipType == "collection");
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection is List<T> list)
            {
                list.AddRange(items);
            }
            else
            {
                foreach (T item in items)
                    collection.Add(item);
            }
        }

        public static void Insert<T>(this ICollection<T> collection, int index, T item)
        {

            if (index < 0 || index > collection.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index was out of range. Must be non-negative and less than the size of the collection.");

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

    }
}

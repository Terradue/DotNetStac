using System;
using Newtonsoft.Json.Linq;
using Stac.Item;

namespace Stac.Extensions
{
    public abstract class AssignableStacExtension : IStacExtension
    {
        private string prefix;
        private IStacObject stacObject;

        public AssignableStacExtension(string prefix)
        {
            this.prefix = prefix;
        }

        public virtual string Id => prefix;

        public IStacObject StacObject { get => stacObject; }

        internal void InitStacObject(IStacObject stacObject)
        {
            this.stacObject = stacObject;
        }

        internal void SetField(string key, object value)
        {
            stacObject.Properties.Remove(key);
            stacObject.Properties.Add(key, value);
        }

        protected object GetField(string fieldName)
        {
            string key = prefix + ":" + fieldName;
            if (!stacObject.Properties.ContainsKey(key))
                return null;
            return stacObject.Properties[key];
        }

        internal T GetField<T>(string fieldName)
        {
            var @object = GetField(fieldName);
            if (@object == null) return default(T);
            if (@object is JToken)
                return (@object as JToken).ToObject<T>();
            return (T)@object;
        }

    }
}
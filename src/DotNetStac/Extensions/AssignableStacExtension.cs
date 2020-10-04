using System;
using Newtonsoft.Json.Linq;
using Stac.Item;

namespace Stac.Extensions
{
    public abstract class AssignableStacExtension : IStacExtension
    {
        private string prefix;
        private IStacObject stacObject;

        public AssignableStacExtension(string prefix, IStacObject stacObject)
        {
            this.prefix = prefix;
            this.stacObject = stacObject;
            StacObject.StacExtensions.Remove(prefix);
            StacObject.StacExtensions.Add(prefix, this);
        }

        public virtual string Id => prefix;

        public IStacObject StacObject
        {
            get
            {
                if (stacObject == null)
                    throw new ExtensionNotAssignedException("Extension {0} is not assigned to a Stac object", Id);
                return stacObject;
            }
        }

        internal void InitStacObject(IStacObject stacObject)
        {
            this.stacObject = stacObject;
        }

        internal void SetField(string key, object value)
        {
            StacObject.Properties.Remove(prefix + ":" + key);
            StacObject.Properties.Add(prefix + ":" + key, value);
        }

        protected object GetField(string fieldName)
        {
            string key = prefix + ":" + fieldName;
            if (!StacObject.Properties.ContainsKey(key))
                return null;
            return StacObject.Properties[key];
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
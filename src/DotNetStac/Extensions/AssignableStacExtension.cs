using System;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Stac.Item;

namespace Stac.Extensions
{
    public abstract class AssignableStacExtension : IStacExtension
    {
        private string prefix;
        private IStacPropertiesContainer stacPropertiesContainer;

        public AssignableStacExtension(string prefix, IStacObject stacObject) : this(prefix, stacObject as IStacPropertiesContainer)
        {
        }

        public AssignableStacExtension(string prefix, IStacPropertiesContainer stacPropertiesContainer)
        {
            this.prefix = prefix;
            this.stacPropertiesContainer = stacPropertiesContainer;
            if (stacPropertiesContainer is IStacObject)
                InitStacObject(stacPropertiesContainer as IStacObject);

        }

        public virtual string Id => prefix;

        public IStacObject StacObject
        {
            get
            {
                if (!(StacPropertiesContainer is IStacObject))
                    throw new ExtensionNotAssignedException("Extension {0} is not assigned to a Stac object", Id);
                return stacPropertiesContainer as IStacObject;
            }
        }

        public IStacPropertiesContainer StacPropertiesContainer
        {
            get
            {
                if (stacPropertiesContainer == null)
                    throw new ExtensionNotAssignedException("Extension {0} is not assigned to a Stac properties container", Id);
                return stacPropertiesContainer;
            }
        }

        public void InitStacObject(IStacObject stacObject)
        {
            stacObject.StacExtensions.Remove(prefix);
            stacObject.StacExtensions.Add(prefix, this);
        }

        protected void SetField(string key, object value)
        {
            StacPropertiesContainer.Properties.Remove(prefix + ":" + key);
            StacPropertiesContainer.Properties.Add(prefix + ":" + key, value);
        }

        protected object GetField(string fieldName)
        {
            string key = prefix + ":" + fieldName;
            if (!StacPropertiesContainer.Properties.ContainsKey(key))
                return null;
            return StacPropertiesContainer.Properties[key];
        }

        protected T GetField<T>(string fieldName)
        {
            var @object = GetField(fieldName);
            if (@object == null) return default(T);
            if (@object is JToken)
                return (@object as JToken).ToObject<T>();
            if (typeof(T).GetTypeInfo().IsEnum)
                return (T)Enum.Parse(typeof(T), @object.ToString());
            return (T)Convert.ChangeType(@object, typeof(T));
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Stac.Collection;
using Stac.Exceptions;

namespace Stac.Extensions
{
    public abstract class StacPropertiesContainerExtension : IStacExtension
    {
        private readonly string identifier;

        public StacPropertiesContainerExtension(string identifier, IStacPropertiesContainer stacPropertiesContainer)
        {
            this.identifier = identifier;
            StacPropertiesContainer = stacPropertiesContainer;
        }

        public virtual string Identifier => identifier;

        public IStacPropertiesContainer StacPropertiesContainer { get; private set; }

        public abstract IDictionary<string, Type> ItemFields { get; }

        public virtual IDictionary<string, Func<IEnumerable<object>, IStacSummaryItem>> GetSummaryFunctions(){
            return ItemFields.ToDictionary(k => k.Key,
                    k => {
                        if ( k.Value == typeof(bool) || k.Value == typeof(short) || k.Value == typeof(int) || k.Value == typeof(long) ||
                             k.Value == typeof(float) || k.Value == typeof(double) || k.Value == typeof(DateTime))
                    });
        }

        protected void DeclareStacExtension()
        {
            if (!StacPropertiesContainer.StacObjectContainer.StacExtensions.Contains(Identifier))
                StacPropertiesContainer.StacObjectContainer.StacExtensions.Add(Identifier);
        }
    }
}
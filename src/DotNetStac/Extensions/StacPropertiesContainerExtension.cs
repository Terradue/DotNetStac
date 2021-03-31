using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
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

        protected void DeclareStacExtension()
        {
            if (!StacPropertiesContainer.StacObjectContainer.StacExtensions.Contains(Identifier))
                StacPropertiesContainer.StacObjectContainer.StacExtensions.Add(Identifier);
        }
    }
}
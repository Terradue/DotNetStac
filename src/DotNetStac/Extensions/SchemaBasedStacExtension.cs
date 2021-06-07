using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Stac.Exceptions;
using Stac.Schemas;

namespace Stac.Extensions
{
    public class SchemaBasedStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        private readonly IStacObject stacObject;

        public SchemaBasedStacExtension(Uri jsonSchema,
                                        IStacObject stacObject) : base(jsonSchema.ToString(),
                                                                       stacObject)
        {
            Preconditions.CheckNotNull<Uri>(jsonSchema, "jsonSchema");
            JsonSchema = jsonSchema;
            this.stacObject = stacObject;
        }

        public SchemaBasedStacExtension(Uri schemaUri,
                                           StacSchemaResolver stacSchemaResolver,
                                           IStacObject stacObject) : base(schemaUri.ToString(),
                                                                       stacObject)
        {
            Preconditions.CheckNotNull<Uri>(schemaUri, "schemaUri");
            this.stacObject = stacObject;
            JsonSchema = schemaUri;
        }


        public static SchemaBasedStacExtension Create(string shortcut,
                                                      StacSchemaResolver stacSchemaResolver,
                                                      IStacObject stacObject)
        {
            if (StacSchemaResolver.CoreTypes.Contains(shortcut))
                throw new Exceptions.InvalidStacSchemaException(shortcut + "is not an extension");
            Uri schema = new Uri($"https://stac-extensions.github.io/{shortcut}/v1.0.0/schema.json");

            return new SchemaBasedStacExtension(schema, stacObject);
        }

        public override string Identifier => JsonSchema.ToString();

        public Uri JsonSchema { get; }

        public override IDictionary<string, Type> ItemFields => new Dictionary<string, Type>();
    }
}

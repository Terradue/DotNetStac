using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Stac.Exceptions;

namespace Stac.Extensions
{
    public class SchemaBasedStacExtension : StacPropertiesContainerExtension, IStacExtension 
    {
        private readonly IStacObject stacObject;

        public SchemaBasedStacExtension(JSchema jsonSchema,
                                        IStacObject stacObject) : base(jsonSchema.Id.ToString(),
                                                                       stacObject)
        {
            Preconditions.CheckNotNull<JSchema>(jsonSchema, "jsonSchema");
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
            JsonSchema = stacSchemaResolver.LoadSchema(schemaUri.ToString());
        }


        public static SchemaBasedStacExtension Create(string shortcut,
                                                      StacSchemaResolver stacSchemaResolver,
                                                      IStacObject stacObject)
        {
            if ( StacSchemaResolver.CoreTypes.Contains(shortcut) )
                throw new Exceptions.InvalidStacSchemaException(shortcut + "is not an extension");
            var jsonSchema = stacSchemaResolver.LoadSchema(version: stacObject.StacVersion.ToString(), shortcut: shortcut);
            return new SchemaBasedStacExtension(jsonSchema, stacObject);
        }

        public override string Identifier => JsonSchema.Id.ToString();

        public JSchema JsonSchema { get; }

        public override IDictionary<string, Type> ItemFields => new Dictionary<string, Type>();
    }
}
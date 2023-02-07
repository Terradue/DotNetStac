// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SchemaBasedStacExtension.cs

using System;
using System.Collections.Generic;
using System.Linq;
using Stac.Exceptions;
using Stac.Schemas;

namespace Stac.Extensions
{
    public class SchemaBasedStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        private readonly IStacObject _stacObject;

        public SchemaBasedStacExtension(
            Uri jsonSchema,
            IStacObject stacObject)
            : base(
                jsonSchema.ToString(),
                stacObject)
        {
            Preconditions.CheckNotNull(jsonSchema, "jsonSchema");
            this.JsonSchema = jsonSchema;
            this._stacObject = stacObject;
        }

        public SchemaBasedStacExtension(
            Uri schemaUri,
            StacSchemaResolver stacSchemaResolver,
            IStacObject stacObject)
            : base(
                schemaUri.ToString(),
                stacObject)
        {
            Preconditions.CheckNotNull(schemaUri, "schemaUri");
            this._stacObject = stacObject;
            this.JsonSchema = schemaUri;
        }

        /// <inheritdoc/>
        public override string Identifier => this.JsonSchema.ToString();

        public Uri JsonSchema { get; }

        /// <inheritdoc/>
        public override IDictionary<string, Type> ItemFields => new Dictionary<string, Type>();

        public static SchemaBasedStacExtension Create(
            string shortcut,
            StacSchemaResolver stacSchemaResolver,
            IStacObject stacObject)
        {
            if (StacSchemaResolver.CoreTypes.Contains(shortcut))
            {
                throw new InvalidStacSchemaException(shortcut + "is not an extension");
            }

            Uri schema = new Uri($"https://stac-extensions.github.io/{shortcut}/v1.0.0/schema.json");

            return new SchemaBasedStacExtension(schema, stacObject);
        }
    }
}

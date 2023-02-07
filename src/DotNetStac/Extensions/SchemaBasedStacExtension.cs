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
    /// <summary>
    /// A schema based extension
    /// </summary>
    public class SchemaBasedStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        private readonly IStacObject _stacObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaBasedStacExtension"/> class.
        /// </summary>
        /// <param name="jsonSchema">The json schema.</param>
        /// <param name="stacObject">The stac object.</param>
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

        /// <summary>
        ///  Initializes a new instance of the <see cref="SchemaBasedStacExtension"/> class.
        /// </summary>
        /// <param name="schemaUri">The schema URI.</param>
        /// <param name="stacSchemaResolver">The stac schema resolver.</param>
        /// <param name="stacObject">The stac object.</param>
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

        /// <summary>
        /// Gets the json schema.
        /// </summary>
        /// <returns>The schema URI</returns>
        public Uri JsonSchema { get; }

        /// <inheritdoc/>
        public override IDictionary<string, Type> ItemFields => new Dictionary<string, Type>();

        /// <summary>
        /// Creates a new instance of the <see cref="SchemaBasedStacExtension"/> class.
        /// </summary>
        /// <param name="shortcut">The shortcut.</param>
        /// <param name="stacSchemaResolver">The stac schema resolver.</param>
        /// <param name="stacObject">The stac object.</param>
        /// <returns>The extension</returns>
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

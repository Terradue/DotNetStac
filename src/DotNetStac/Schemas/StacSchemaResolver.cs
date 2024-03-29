﻿// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacSchemaResolver.cs

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Schema;

namespace Stac.Schemas
{
    /// <summary>
    /// Stac Schema Resolver.
    /// </summary>
    public class StacSchemaResolver
    {
        /// <summary>
        /// Core types.
        /// </summary>
        public static readonly string[] CoreTypes = new string[] { "item", "catalog", "collection" };

        private static readonly IDictionary<string, Uri> SchemaMap = new Dictionary<string, Uri>();

        private readonly JSchemaResolver _jsonSchemaResolver;
        private readonly IDictionary<string, JSchema> _schemaCompiled;

        /// <summary>
        /// Initializes a new instance of the <see cref="StacSchemaResolver"/> class.
        /// </summary>
        /// <param name="jsonSchemaResolver">Json schema resolver.</param>
        public StacSchemaResolver(JSchemaResolver jsonSchemaResolver)
        {
            this._jsonSchemaResolver = jsonSchemaResolver;
            this._schemaCompiled = new Dictionary<string, JSchema>();
        }

        /// <summary>
        /// Loads the schema from url or shortcut.
        /// </summary>
        /// <param name="baseUrl">Base url.</param>
        /// <param name="version">Version.</param>
        /// <param name="shortcut">Shortcut.</param>
        /// <returns>The schema.</returns>
        public JSchema LoadSchema(string baseUrl = null, string version = null, string shortcut = null)
        {
            string vversion = string.IsNullOrEmpty(version) ? "unversioned" : "v" + version;
            Uri baseUri = null;
            if (string.IsNullOrEmpty(baseUrl))
            {
                baseUri = new Uri($"https://schemas.stacspec.org/{vversion}/");
            }
            else
            {
                baseUri = new Uri(baseUrl);
            }

            Uri schemaUri = null;
            if (shortcut == "item" || shortcut == "catalog" || shortcut == "collection")
            {
                schemaUri = new Uri(baseUri, $"{shortcut}-spec/json-schema/{shortcut}.json");
            }
            else if (!string.IsNullOrEmpty(shortcut))
            {
                if (shortcut == "proj")
                {
                    // Capture a very common mistake and give a better explanation (see #4)
                    throw new Exception("'stac_extensions' must contain 'projection instead of 'proj'.");
                }

                schemaUri = new Uri(baseUri, $"extensions/{shortcut}/json-schema/schema.json");
            }
            else
            {
                schemaUri = baseUri;
            }

            if (!string.IsNullOrEmpty(baseUrl) && SchemaMap.ContainsKey(baseUrl))
            {
                schemaUri = SchemaMap[baseUrl];
            }

            if (this._schemaCompiled.ContainsKey(schemaUri.ToString()))
            {
                return this._schemaCompiled[schemaUri.ToString()];
            }
            else
            {
                Stream stream = null;
                try
                {
                    stream = this._jsonSchemaResolver.GetSchemaResource(null, new SchemaReference() { BaseUri = schemaUri });
                }
                catch (Exception e)
                {
                    throw new Exceptions.InvalidStacSchemaException(string.Format("Error getting schema at Uri '{0}'", schemaUri), e);
                }

                var sr = new StreamReader(stream);
                this._schemaCompiled[schemaUri.ToString()] = JSchema.Parse(sr.ReadToEnd(), this._jsonSchemaResolver);
                return this._schemaCompiled[schemaUri.ToString()];
            }
        }
    }
}

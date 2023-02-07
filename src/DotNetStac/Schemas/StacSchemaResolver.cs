// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacSchemaResolver.cs

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Schema;
using Stac.Extensions.ItemCollections;

namespace Stac.Schemas
{
    public class StacSchemaResolver
    {
        private readonly JSchemaResolver _jSchemaResolver;
        private readonly IDictionary<string, JSchema> _schemaCompiled;

        public static string[] CoreTypes = new string[] { "item", "catalog", "collection" };

        public StacSchemaResolver(JSchemaResolver jSchemaResolver)
        {
            this._jSchemaResolver = jSchemaResolver;
            this._schemaCompiled = new Dictionary<string, JSchema>();
        }

        private static readonly IDictionary<string, Uri> schemaMap = new Dictionary<string, Uri>();

        public JSchema LoadSchema(string baseUrl = null, string version = null, string shortcut = null)
        {
            string vversion = string.IsNullOrEmpty(version) ? "unversioned" : "v" + version;
            Uri baseUri = null;
            if (string.IsNullOrEmpty(baseUrl))
            {
                baseUri = new Uri($"https://schemas.stacspec.org/{vversion}/");
            }
            else
                baseUri = new Uri(baseUrl);

            Uri schemaUri = null;
            if (shortcut == "item" || shortcut == "catalog" || shortcut == "collection")
                schemaUri = new Uri(baseUri, $"{shortcut}-spec/json-schema/{shortcut}.json");
            else if (shortcut == "item-collection")
                return ItemCollection.GenerateJSchema(version);
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

            if (!string.IsNullOrEmpty(baseUrl) && schemaMap.ContainsKey(baseUrl))
            {
                schemaUri = schemaMap[baseUrl];
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
                    stream = this._jSchemaResolver.GetSchemaResource(null, new SchemaReference() { BaseUri = schemaUri });
                }
                catch (Exception e)
                {
                    throw new Exceptions.InvalidStacSchemaException(string.Format("Error getting schema at Uri '{0}'", schemaUri), e);
                }

                var sr = new StreamReader(stream);
                this._schemaCompiled[schemaUri.ToString()] = JSchema.Parse(sr.ReadToEnd(), this._jSchemaResolver);
                return this._schemaCompiled[schemaUri.ToString()];
            }
        }
    }
}

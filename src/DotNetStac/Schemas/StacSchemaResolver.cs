using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Stac.Schemas
{
    public class StacSchemaResolver
    {
        private readonly JSchemaResolver jSchemaResolver;
        private IDictionary<string, JSchema> schemaCompiled;

        public static string[] CoreTypes = new string[] { "item", "catalog", "collection" };

        public StacSchemaResolver(JSchemaResolver jSchemaResolver)
        {
            this.jSchemaResolver = jSchemaResolver;
            this.schemaCompiled = new Dictionary<string, JSchema>();
        }

        private static IDictionary<string, Uri> schemaMap = new Dictionary<string, Uri>();

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
            bool isExtension = false;
            if (shortcut == "item" || shortcut == "catalog" || shortcut == "collection")
                schemaUri = new Uri(baseUri, $"{shortcut}-spec/json-schema/{shortcut}.json");
            else if (!string.IsNullOrEmpty(shortcut))
            {
                if (shortcut == "proj")
                {
                    // Capture a very common mistake and give a better explanation (see #4)
                    throw new Exception("'stac_extensions' must contain 'projection instead of 'proj'.");
                }
                schemaUri = new Uri(baseUri, $"extensions/{shortcut}/json-schema/schema.json");
                isExtension = true;
            }
            else
            {
                schemaUri = baseUri;
            }

            if (!string.IsNullOrEmpty(baseUrl) && schemaMap.ContainsKey(baseUrl))
            {
                schemaUri = schemaMap[baseUrl];
            }

            if (schemaCompiled.ContainsKey(schemaUri.ToString()))
            {
                return schemaCompiled[schemaUri.ToString()];
            }
            else
            {
                Stream stream = null;
                try
                {
                    stream = jSchemaResolver.GetSchemaResource(null, new SchemaReference() { BaseUri = schemaUri });
                }
                catch (Exception e)
                {
                    throw new Stac.Exceptions.InvalidStacSchemaException(string.Format("Error getting schema at Uri '{0}'", schemaUri), e);
                }
                var sr = new StreamReader(stream);
                schemaCompiled[schemaUri.ToString()] = JSchema.Parse(sr.ReadToEnd(), jSchemaResolver);
                return schemaCompiled[schemaUri.ToString()];
            }
        }
    }
}
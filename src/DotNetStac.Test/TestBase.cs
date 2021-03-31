using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Stac.Test
{
    public abstract class TestBase
    {
        private static readonly Assembly ThisAssembly = typeof(TestBase)
#if NETCOREAPP1_1
        .GetTypeInfo()
#endif
        .Assembly;
        private static readonly string AssemblyName = ThisAssembly.GetName().Name;
        private static IDictionary<string, Uri> schemaMap = new Dictionary<string, Uri>();

        private IDictionary<string, JSchema> schemaCompiled;

        protected HttpClient httpClient = new HttpClient();
        private static string[] coreObjects = new string[] { "item", "catalog", "collection" };

        protected TestBase()
        {
            schemaCompiled = new Dictionary<string, JSchema>();
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = ThisAssembly.CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        protected string GetJson(string folder, [CallerMemberName] string name = null)
        {
            var type = GetType().Name;
            var path = Path.Combine(AssemblyDirectory, @"../../..", "Resources", folder, type + "_" + name + ".json");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("file not found at " + path);
            }

            return File.ReadAllText(path);
        }

        protected Uri GetUri(string folder, [CallerMemberName] string name = null)
        {
            var type = GetType().Name;
            var path = Path.Combine(AssemblyDirectory, @"../../..", "Resources", folder, type + "_" + name + ".json");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("file not found at " + path);
            }

            return new Uri(path);
        }

        protected Uri GetUseCaseFileUri(string name)
        {
            var type = GetType().Name;
            var path = Path.Combine(AssemblyDirectory, @"../../..", "Resources/UseCases", type, name);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("file not found at " + path);
            }

            return new Uri(path);
        }

        protected string GetUseCaseJson(string name)
        {
            var type = GetType().Name;
            var path = Path.Combine(AssemblyDirectory, @"../../..", "Resources/UseCases", type, name);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("file not found at " + path);
            }

            return File.ReadAllText(path);
        }

        public bool ValidateJson(string jsonstr)
        {
            JObject json = JObject.Parse(jsonstr);
            return ValidateJObject(json);
        }

        private bool ValidateJObject(JObject jObject)
        {
            string type = null;
            if (jObject.Value<string>("type") == "Feature")
            {
                type = "item";
            }
            else if (jObject.Value<string>("type") == "FeatureCollection")
            {
                // type = 'itemcollection';
                throw new NotSupportedException($"{jObject.Value<string>("id")}. Skipping; STAC ItemCollections not supported yet");
            }
            else if (jObject.Value<string>("type") == "Collection" || jObject["extent"] != null || jObject["license"] != null)
            {
                type = "collection";
            }
            else if (jObject.Value<string>("type") == "Catalog" || jObject["description"] != null)
            {
                type = "catalog";
            }
            else
            {
                throw new InvalidDataException($"{jObject.Value<string>("id")}. Error; Unknown data");
            }

            // Get all schema to validate against
            List<string> schemas = new List<string>() { type };
            if (jObject.Value<JArray>("stac_extensions") != null)
                schemas.AddRange(jObject.Value<JArray>("stac_extensions").Select(a => a.Value<string>()));

            foreach (var schema in schemas)
            {
                IList<ValidationError> errorMessages = null;
                string shortcut = null, baseUrl = null;
                if (Uri.IsWellFormedUriString(schema, UriKind.Absolute))
                    baseUrl = schema;
                else
                    shortcut = schema;
                if (jObject.IsValid(LoadSchema(baseUrl: baseUrl, shortcut: shortcut, version: jObject["stac_version"].Value<string>()), out errorMessages))
                    continue;

                throw new InvalidDataException(schema + ":" + string.Join("\n", errorMessages.
                        Select(e => FormatMessage(e, "", new StringBuilder(e.Message)))));
            }
            return true;
        }

        public JSchema LoadSchema(string baseUrl = null, string version = null, string shortcut = null)
        {
            string vversion = string.IsNullOrEmpty(version) ? "unversioned" : "v" + version;
            Uri baseUri = null;
            if (string.IsNullOrEmpty(baseUrl))
            {
                if (coreObjects.Contains(shortcut))
                    baseUri = new Uri($"https://schemas.stacspec.org/{vversion}/");
                else
                    baseUri = new Uri($"https://schemas.stacspec.org/");
            }
            else
                baseUri = new Uri(baseUrl);

            Uri schemaUri = null;
            // bool isExtension = false;
            if (shortcut == "item" || shortcut == "catalog" || shortcut == "collection")
                schemaUri = new Uri(baseUri, $"{shortcut}-spec/json-schema/{shortcut}.json");
            else if (!string.IsNullOrEmpty(shortcut))
            {
                if (shortcut == "proj")
                {
                    // Capture a very common mistake and give a better explanation (see #4)
                    throw new Exception("'stac_extensions' must contain 'projection instead of 'proj'.");
                }
                schemaUri = new Uri(baseUri, $"extensions/{shortcut}/json-schema/schema.json`");
                // isExtension = true;
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
                try
                {
                    schemaCompiled[schemaUri.ToString()] = JSchema.Parse(httpClient.GetStringAsync(schemaUri).GetAwaiter().GetResult(), new JSchemaUrlResolver());
                    return schemaCompiled[schemaUri.ToString()];
                }
                catch (HttpRequestException hre)
                {
                    throw new Exception(schemaUri + ":" + hre.Message, hre);
                }
            }
        }

        internal static string FormatMessage(ValidationError validationError, string prefix, StringBuilder message)
        {
            message.Append(validationError.Message);
            if (message[message.Length - 1] != '.')
            {
                message.Append('.');
            }
            message.Append('\n' + prefix);

            message.Append("Path '");
            message.Append(validationError.Path);
            message.Append('\'');

            message.Append('.');

            if (validationError.ChildErrors != null)
            {
                foreach (ValidationError childError in validationError.ChildErrors)
                {
                    FormatMessage(childError, prefix + "  ", message);
                }
            }

            return message.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Stac.Exceptions;
using Stac.Extensions.ItemCollections;
using Stac.Schemas;

namespace Stac.Schemas
{
    public class StacValidator
    {
        private StacSchemaResolver schemaResolver = null;

        private Dictionary<Type, string> stacTypes = new Dictionary<Type, string>();

        public StacValidator(JSchemaUrlResolver jSchemaUrlResolver)
        {
            this.schemaResolver = new StacSchemaResolver(jSchemaUrlResolver);
            this.stacTypes.Add(typeof(StacItem), "item");
            this.stacTypes.Add(typeof(StacCatalog), "catalog");
            this.stacTypes.Add(typeof(StacCollection), "collection");
            this.stacTypes.Add(typeof(ItemCollection), "item-collection");
        }

        /// <summary>
        /// Validate a json string against its STAC schema specification
        /// </summary>
        /// <param name="jsonstr"></param>
        /// <returns>true when valid</returns>
        public bool ValidateJson(string jsonstr)
        {
            JObject jobject;
            using (var reader = new JsonTextReader(new StringReader(jsonstr)) { DateTimeZoneHandling = DateTimeZoneHandling.Utc })
                jobject = JObject.Load(reader);
            return ValidateJObject(jobject);
        }

        private bool ValidateJObject(JObject jObject)
        {
            Type stacType = Utils.IdentifyStacType(jObject);

            // Get all schema to validate against
            List<string> schemas = new List<string>() { stacTypes[stacType] };
            if (jObject.Value<JArray>("stac_extensions") != null)
                schemas.AddRange(jObject.Value<JArray>("stac_extensions").Select(a => a.Value<string>()));

            foreach (var schema in schemas)
            {
                string shortcut = null, baseUrl = null;
                if (Uri.IsWellFormedUriString(schema, UriKind.Absolute))
                    baseUrl = schema;
                else
                    shortcut = schema;

                if (!jObject.ContainsKey("stac_version"))
                    throw new InvalidStacDataException("Missing 'stac_version' property");

                var jsonSchema = schemaResolver.LoadSchema(baseUrl: baseUrl, shortcut: shortcut, version: jObject["stac_version"].Value<string>());
                if (jObject.IsValid(jsonSchema, out IList<ValidationError> errorMessages))
                    continue;

                throw new InvalidStacDataException(schema + ":\n" + string.Join("\n", errorMessages.
                        Select(e => FormatMessage(e, "", new StringBuilder()))));
            }
            return true;
        }

        internal static string FormatMessage(ValidationError validationError, string prefix, StringBuilder message)
        {
            message.AppendFormat("{0},{1} Path '", validationError.LineNumber, validationError.LinePosition);

            message.Append(validationError.Path);
            message.Append("': ");

            message.Append(validationError.Message);
            if (message[message.Length - 1] != '.')
            {
                message.Append('.');
            }

            if (validationError.ChildErrors != null && validationError.ChildErrors.Count > 0)
            {
                message.Append('\n' + prefix);
                foreach (ValidationError childError in validationError.ChildErrors)
                {
                    FormatMessage(childError, prefix + "  ", message);
                }
                if (message[message.Length - 1] != '\n')
                    message.Append('\n');
            }

            return message.ToString();
        }
    }
}

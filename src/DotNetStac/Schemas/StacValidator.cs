// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacValidator.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Stac.Exceptions;

namespace Stac.Schemas
{
    /// <summary>
    /// Stac Validator.
    /// </summary>
    public class StacValidator
    {
        private readonly StacSchemaResolver _schemaResolver = null;

        private readonly Dictionary<Type, string> _stacTypes = new Dictionary<Type, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="StacValidator"/> class.
        /// </summary>
        /// <param name="jsonSchemaUrlResolver">Json schema resolver.</param>
        public StacValidator(JSchemaUrlResolver jsonSchemaUrlResolver)
        {
            this._schemaResolver = new StacSchemaResolver(jsonSchemaUrlResolver);
            this._stacTypes.Add(typeof(StacItem), "item");
            this._stacTypes.Add(typeof(StacCatalog), "catalog");
            this._stacTypes.Add(typeof(StacCollection), "collection");
        }

        /// <summary>
        /// Validate a json string against its STAC schema specification
        /// </summary>
        /// <param name="jsonstr">json string</param>
        /// <returns>true when valid</returns>
        public bool ValidateJson(string jsonstr)
        {
            using (var reader = new JsonTextReader(new StringReader(jsonstr)) { DateTimeZoneHandling = DateTimeZoneHandling.Utc })
            {
                this.DetectDuplicateKeys(reader);
            }

            JObject jobject;
            using (var reader = new JsonTextReader(new StringReader(jsonstr)) { DateTimeZoneHandling = DateTimeZoneHandling.Utc })
            {
                jobject = JObject.Load(reader);
            }

            return this.ValidateJObject(jobject);
        }

        internal static string FormatMessage(ValidationError validationError, string prefix)
        {
            StringBuilder message = new StringBuilder();
            message.Append(prefix);
            if (validationError.LineNumber > 1 && validationError.LinePosition > 1)
            {
                message.AppendFormat("[{0},{1}]", validationError.LineNumber, validationError.LinePosition);
            }
            else
            {
                message.AppendFormat("[ROOT]");
            }

            if (!string.IsNullOrEmpty(validationError.Path))
            {
                message.AppendFormat(" Path '{0}'", validationError.Path);
            }

            message.Append(": ");

            message.Append(validationError.Message);
            if (message[message.Length - 1] != '.')
            {
                message.Append('.');
            }

            if (validationError.ChildErrors != null && validationError.ChildErrors.Count > 0)
            {
                foreach (ValidationError childError in validationError.ChildErrors)
                {
                    message.Append('\n' + prefix);
                    message.Append(FormatMessage(childError, prefix + "  "));
                }
            }

            return message.ToString();
        }

        private bool DetectDuplicateKeys(JsonReader jobject)
        {
            var stack = new Stack<string>();
            while (jobject.Read())
            {
                switch (jobject.TokenType)
                {
                    case JsonToken.StartObject:
                        this.DetectDuplicateKeys(jobject);
                        break;
                    case JsonToken.PropertyName:
                        var propertyName = jobject.Value.ToString();
                        if (stack.Contains(propertyName))
                        {
                            throw new InvalidStacDataException($"Duplicate key {propertyName} found in JSON: " + jobject.Path);
                        }

                        stack.Push(propertyName);
                        break;
                    case JsonToken.EndObject:
                        return true;
                }
            }

            return true;
        }

        private bool ValidateJObject(JObject jsonObject)
        {
            Type stacType = Utils.IdentifyStacType(jsonObject);

            // Get all schema to validate against
            List<string> schemas = new List<string>() { this._stacTypes[stacType] };
            if (jsonObject.Value<JArray>("stac_extensions") != null)
            {
                schemas.AddRange(jsonObject.Value<JArray>("stac_extensions").Select(a => a.Value<string>()));
            }

            foreach (var schema in schemas)
            {
                string shortcut = null, baseUrl = null;
                if (Uri.IsWellFormedUriString(schema, UriKind.Absolute))
                {
                    baseUrl = schema;
                }
                else
                {
                    shortcut = schema;
                }

                if (!jsonObject.ContainsKey("stac_version"))
                {
                    throw new InvalidStacDataException("Missing 'stac_version' property");
                }

                var jsonSchema = this._schemaResolver.LoadSchema(baseUrl: baseUrl, shortcut: shortcut, version: jsonObject["stac_version"].Value<string>());
                if (jsonObject.IsValid(jsonSchema, out IList<ValidationError> errorMessages))
                {
                    continue;
                }

                throw new InvalidStacDataException(schema + ":\n" + string.Join("\n", errorMessages.
                        Select(e => FormatMessage(e, string.Empty))));
            }

            return true;
        }
    }
}

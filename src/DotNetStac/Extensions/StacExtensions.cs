using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Schema;

namespace Stac.Extensions
{
    public static class StacExtensions
    {
        public static Dictionary<string, Type> ManagedStacExtensions = new Dictionary<string, Type>();

        public static void InitManagedExtensions()
        {
            ManagedStacExtensions.Clear();
            ManagedStacExtensions.Add(Eo.EoStacExtension.JsonSchemaUrl, typeof(Eo.EoStacExtension));
            ManagedStacExtensions.Add("https://schemas.stacspec.org/v1.0.0-rc.1/extensions/eo/json-schema/schema.json#", typeof(Eo.EoStacExtension));
            ManagedStacExtensions.Add("eo", typeof(Eo.EoStacExtension));
            ManagedStacExtensions.Add(Processing.ProcessingStacExtension.JsonSchemaUrl, typeof(Processing.ProcessingStacExtension));
            ManagedStacExtensions.Add("processing", typeof(Processing.ProcessingStacExtension));
            ManagedStacExtensions.Add(Projection.ProjectionStacExtension.JsonSchemaUrl, typeof(Projection.ProjectionStacExtension));
            ManagedStacExtensions.Add("projection", typeof(Projection.ProjectionStacExtension));
            ManagedStacExtensions.Add("https://schemas.stacspec.org/v1.0.0-rc.1/extensions/projection/json-schema/schema.json#", typeof(Projection.ProjectionStacExtension));
            ManagedStacExtensions.Add(Sar.SarStacExtension.JsonSchemaUrl, typeof(Sar.SarStacExtension));
            ManagedStacExtensions.Add("sar", typeof(Sar.SarStacExtension));
            ManagedStacExtensions.Add(Sat.SatStacExtension.JsonSchemaUrl, typeof(Sat.SatStacExtension));
            ManagedStacExtensions.Add("sat", typeof(Sat.SatStacExtension));
            ManagedStacExtensions.Add(View.ViewStacExtension.JsonSchemaUrl, typeof(View.ViewStacExtension));
            ManagedStacExtensions.Add("view", typeof(View.ViewStacExtension));
            ManagedStacExtensions.Add("https://schemas.stacspec.org/v1.0.0-rc.1/extensions/view/json-schema/schema.json#", typeof(View.ViewStacExtension));
        }

        public static IEnumerable<IStacExtension> GetDeclaredExtensions(this IStacPropertiesContainer stacPropertiesContainer)
        {
            if (ManagedStacExtensions.Count == 0) InitManagedExtensions();
            return stacPropertiesContainer.StacObjectContainer.StacExtensions
                    .Select(stacExtension => LoadStacExtension(stacExtension, stacPropertiesContainer.StacObjectContainer));
        }

        private static IStacExtension LoadStacExtension(string stacExtension, IStacObject stacObject)
        {
            if (ManagedStacExtensions.ContainsKey(stacExtension))
            {
                try
                {
                    return Activator.CreateInstance(ManagedStacExtensions[stacExtension], new object[1] { stacObject }) as IStacExtension;
                }
                catch { }
            }

            string shortcut = null, baseUrl = null;
            if (Uri.IsWellFormedUriString(stacExtension, UriKind.Absolute))
                baseUrl = stacExtension;
            else
                shortcut = stacExtension;
            var schema = new StacSchemaResolver(new JSchemaUrlResolver())
                .LoadSchema(baseUrl: baseUrl, shortcut: shortcut, version: stacObject.StacVersion.ToString());

            return new SchemaBasedStacExtension(schema, stacObject);
        }
    }
}
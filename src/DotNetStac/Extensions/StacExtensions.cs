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
            ManagedStacExtensions.Add(Processing.ProcessingStacExtension.JsonSchemaUrl, typeof(Processing.ProcessingStacExtension));
            ManagedStacExtensions.Add(Projection.ProjectionStacExtension.JsonSchemaUrl, typeof(Projection.ProjectionStacExtension));
            ManagedStacExtensions.Add(Sar.SarStacExtension.JsonSchemaUrl, typeof(Sar.SarStacExtension));
            ManagedStacExtensions.Add(Sat.SatStacExtension.JsonSchemaUrl, typeof(Sat.SatStacExtension));
            ManagedStacExtensions.Add(View.ViewStacExtension.JsonSchemaUrl, typeof(View.ViewStacExtension));
        }

        public static IEnumerable<IStacExtension> GetDeclaredExtensions(this IStacPropertiesContainer stacPropertiesContainer)
        {
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

            return new SchemaBasedStacExtension(new StacSchemaResolver(new JSchemaUrlResolver()).LoadSchema(stacExtension), stacObject);
        }
    }
}
// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacExtensions.cs

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Stac.Extensions
{
    /// <summary>
    /// Helper class to access STAC extensions
    /// </summary>
    public static class StacExtensions
    {
        private static Dictionary<string, Type> managedStacExtensions = new Dictionary<string, Type>();
        private static readonly object initLock = new object();

        /// <summary>
        /// Gets dictionary of extensions managed by the library
        /// </summary>
        /// <value></value>
        public static Dictionary<string, Type> ManagedStacExtensions
        {
            get
            {
                if (managedStacExtensions.Count == 0) InitManagedExtensions();
                return managedStacExtensions;
            }
            private set => managedStacExtensions = value;
        }

        /// <summary>
        /// Initialize the managed extensions
        /// </summary>
        public static void InitManagedExtensions()
        {
            lock (initLock)
            {
                managedStacExtensions.Clear();
                managedStacExtensions.Add(Eo.EoStacExtension.JsonSchemaUrl, typeof(Eo.EoStacExtension));
                managedStacExtensions.Add("eo", typeof(Eo.EoStacExtension));
                managedStacExtensions.Add(Processing.ProcessingStacExtension.JsonSchemaUrl, typeof(Processing.ProcessingStacExtension));
                managedStacExtensions.Add("processing", typeof(Processing.ProcessingStacExtension));
                managedStacExtensions.Add(Projection.ProjectionStacExtension.JsonSchemaUrl, typeof(Projection.ProjectionStacExtension));
                managedStacExtensions.Add("projection", typeof(Projection.ProjectionStacExtension));
                managedStacExtensions.Add(Raster.RasterStacExtension.JsonSchemaUrl, typeof(Raster.RasterStacExtension));
                managedStacExtensions.Add(Sar.SarStacExtension.JsonSchemaUrl, typeof(Sar.SarStacExtension));
                managedStacExtensions.Add("sar", typeof(Sar.SarStacExtension));
                managedStacExtensions.Add(Sat.SatStacExtension.JsonSchemaUrl, typeof(Sat.SatStacExtension));
                managedStacExtensions.Add("sat", typeof(Sat.SatStacExtension));
                managedStacExtensions.Add(View.ViewStacExtension.JsonSchemaUrl, typeof(View.ViewStacExtension));
                managedStacExtensions.Add("view", typeof(View.ViewStacExtension));
            }
        }

        /// <summary>
        /// Get the declared extensions for a specific stac properties container
        /// </summary>
        /// <param name="stacPropertiesContainer"></param>
        public static IEnumerable<IStacExtension> GetDeclaredExtensions(this IStacPropertiesContainer stacPropertiesContainer)
        {
            if (ManagedStacExtensions.Count == 0) InitManagedExtensions();
            return stacPropertiesContainer.StacObjectContainer.StacExtensions
                    .Select(stacExtension => LoadStacExtension(stacExtension, stacPropertiesContainer.StacObjectContainer));
        }

        private static IStacExtension LoadStacExtension(string stacExtension, IStacObject stacObject)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            CultureInfo culture = CultureInfo.InvariantCulture; // use InvariantCulture or other if you prefer
            if (ManagedStacExtensions.ContainsKey(stacExtension))
            {
                try
                {
                    return Activator.CreateInstance(ManagedStacExtensions[stacExtension], flags, null, new object[1] { stacObject }, culture) as IStacExtension;
                }
                catch
                {
                }
            }

            Uri schema = null;
            if (Uri.IsWellFormedUriString(stacExtension, UriKind.Absolute))
                schema = new Uri(stacExtension);
            else
                schema = new Uri($"https://stac-extensions.github.io/{stacExtension}/v1.0.0/schema.json");

            return new SchemaBasedStacExtension(schema, stacObject);
        }
    }
}

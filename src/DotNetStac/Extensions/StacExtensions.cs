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
        private static readonly object InitLock = new object();
        private static Dictionary<string, Type> ManagedStacExtensionsDic = new Dictionary<string, Type>();

        /// <summary>
        /// Gets dictionary of extensions managed by the library
        /// </summary>
        /// <value>
        /// Dictionary of extensions managed by the library
        /// </value>
        public static Dictionary<string, Type> ManagedStacExtensions
        {
            get
            {
                if (ManagedStacExtensionsDic.Count == 0)
                {
                    InitManagedExtensions();
                }

                return ManagedStacExtensionsDic;
            }
            private set => ManagedStacExtensionsDic = value;
        }

        /// <summary>
        /// Initialize the managed extensions
        /// </summary>
        public static void InitManagedExtensions()
        {
            lock (InitLock)
            {
                ManagedStacExtensionsDic.Clear();
                ManagedStacExtensionsDic.Add(Eo.EoStacExtension.JsonSchemaUrl, typeof(Eo.EoStacExtension));
                ManagedStacExtensionsDic.Add("eo", typeof(Eo.EoStacExtension));
                ManagedStacExtensionsDic.Add(Processing.ProcessingStacExtension.JsonSchemaUrl, typeof(Processing.ProcessingStacExtension));
                ManagedStacExtensionsDic.Add("processing", typeof(Processing.ProcessingStacExtension));
                ManagedStacExtensionsDic.Add(Projection.ProjectionStacExtension.JsonSchemaUrl, typeof(Projection.ProjectionStacExtension));
                ManagedStacExtensionsDic.Add("projection", typeof(Projection.ProjectionStacExtension));
                ManagedStacExtensionsDic.Add(Raster.RasterStacExtension.JsonSchemaUrl, typeof(Raster.RasterStacExtension));
                ManagedStacExtensionsDic.Add(Sar.SarStacExtension.JsonSchemaUrl, typeof(Sar.SarStacExtension));
                ManagedStacExtensionsDic.Add("sar", typeof(Sar.SarStacExtension));
                ManagedStacExtensionsDic.Add(Sat.SatStacExtension.JsonSchemaUrl, typeof(Sat.SatStacExtension));
                ManagedStacExtensionsDic.Add("sat", typeof(Sat.SatStacExtension));
                ManagedStacExtensionsDic.Add(View.ViewStacExtension.JsonSchemaUrl, typeof(View.ViewStacExtension));
                ManagedStacExtensionsDic.Add("view", typeof(View.ViewStacExtension));
                ManagedStacExtensionsDic.Add(Disaster.DisastersCharterStacExtension.JsonSchemaUrl, typeof(Disaster.DisastersCharterStacExtension));
            }
        }

        /// <summary>
        /// Get the declared extensions for a specific stac properties container
        /// </summary>
        /// <param name="stacPropertiesContainer">The stac properties container</param>
        /// <returns>Collection of extensions</returns>
        public static IEnumerable<IStacExtension> GetDeclaredExtensions(this IStacPropertiesContainer stacPropertiesContainer)
        {
            if (ManagedStacExtensions.Count == 0)
            {
                InitManagedExtensions();
            }

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
            {
                schema = new Uri(stacExtension);
            }
            else
            {
                schema = new Uri($"https://stac-extensions.github.io/{stacExtension}/v1.0.0/schema.json");
            }

            return new SchemaBasedStacExtension(schema, stacObject);
        }
    }
}

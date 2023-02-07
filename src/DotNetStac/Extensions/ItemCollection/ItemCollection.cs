// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ItemCollection.cs

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Stac.Collection;

namespace Stac.Extensions.ItemCollections
{
    public class ItemCollection : StacCollection, IStacExtension
    {

        public const string JsonSchemaUrl = "https://stac-extensions.github.io/processing/v1.0.0/schema.json";

        public ItemCollection(
            string id,
                              string description,
                              List<StacItem> stacItems)
            : base(
                id,
                                                               description,
                                                               null)
        {
            if (stacItems != null)
            {
                this.Features = new List<StacItem>(stacItems);
                this.Extent = StacExtent.Create(stacItems);
            }
        }

        /// <summary>
        /// Gets sTAC type (FeatureCollection)
        /// </summary>
        /// <value>
        /// STAC type (FeatureCollection)
        /// </value>
        [JsonProperty("type")]
        public override string Type => "FeatureCollection";

        [JsonProperty(PropertyName = "features", Required = Required.Always)]
        public List<StacItem> Features { get; set; }

        /// <inheritdoc/>
        public string Identifier => JsonSchemaUrl;

        /// <inheritdoc/>
        public bool IsDeclared => true;

        /// <inheritdoc/>
        public IDictionary<string, ISummaryFunction> GetSummaryFunctions()
        {
            return new Dictionary<string, ISummaryFunction>();
        }

        internal static JSchema GenerateJSchema(string version)
        {

            JSchema jSchema = new JSchema();
            jSchema.SchemaVersion = new Uri("http://json-schema.org/draft-07/schema#");
            jSchema.Title = "STAC ItemCollection Extension";
            jSchema.Type = JSchemaType.Object;
            jSchema.Description = "This object represents the metadata for a set of items in a SpatioTemporal Asset Catalog.";
            jSchema.AllowAdditionalProperties = true;

            JSchema featureCollectionRef = new JSchema();

            featureCollectionRef.Ref = GetSchema(new Uri("https://geojson.org/schema/FeatureCollection.json"));
            JSchema fcr = new JSchema();
            fcr.OneOf.Add(featureCollectionRef);

            jSchema.AllOf.Add(fcr);

            JSchema itemSchema = GetSchema(new Uri("https://schemas.stacspec.org/v" + version + "/item-spec/json-schema/item.json"));
            JSchema linkSchema = JSchema.Parse(itemSchema.ExtensionData["definitions"]["link"].ToString());

            jSchema.AllOf.Add(new JSchema()
            {
                Properties = {
                    { "stac_version", new JSchema(){
                        Title = "STAC version",
                        Type = JSchemaType.String
                    }},
                    { "stac_extensions", new JSchema(){
                        Title = "STAC extensions",
                        Type = JSchemaType.Array,
                        UniqueItems = true,
                        Items = { new JSchema { Type = JSchemaType.String, Format = "uri", Title = "Reference to a JSON Schema" } }
                    }},
                    { "features", new JSchema(){
                        Title = "ItemCollection features",
                        Type = JSchemaType.Array,
                        Items = { new JSchema { Ref = itemSchema } }
                    }},
                    { "links", new JSchema(){
                        Title = "Links",
                        Type = JSchemaType.Array,
                        Items = { new JSchema { Ref = linkSchema } }
                    }},
                }
            });

            return jSchema;
        }

        private static JSchema GetSchema(Uri schemaUri)
        {
            JSchemaUrlResolver jSchemaResolver = new JSchemaUrlResolver();
            Stream stream = null;
            try
            {
                stream = jSchemaResolver.GetSchemaResource(null, new SchemaReference() { BaseUri = schemaUri });
            }
            catch (Exception e)
            {
                throw new Exceptions.InvalidStacSchemaException(string.Format("Error getting schema at Uri '{0}'", schemaUri), e);
            }

            var sr = new StreamReader(stream);
            return JSchema.Parse(sr.ReadToEnd(), jSchemaResolver);
        }
    }
}

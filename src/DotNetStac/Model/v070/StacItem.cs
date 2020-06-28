using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DotNetStac;
using DotNetStac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Converters;
using Stac.Extensions;

namespace Stac.Model.v070
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    internal class StacItem070 : GeoJSON.Net.Feature.Feature, IStacObject
    {
        private Collection<StacLink> links;

        private Dictionary<string, StacAsset> assets;

        private string stacVersion = StacVersionList.Current;

        private Collection<IStacExtension> extensions;
        private string collection;

        [JsonConstructor]
        public StacItem070(IGeometryObject geometry, IDictionary<string, object> properties = null, string id = null) : base(geometry, properties, id)
        { }

        public StacItem070(IGeometryObject geometry, object properties, string id = null) : base(geometry, properties, id)
        { }

        [JsonProperty("stac_extensions")]
        [JsonConverter(typeof(StacExtensionConverter))]
        public Collection<IStacExtension> StacExtensions
        {
            get
            {
                if (extensions == null)
                    extensions = new Collection<IStacExtension>();
                return extensions;
            }
            set
            {
                extensions = value;
            }
        }

        [JsonProperty("stac_version")]
        public string StacVersion
        {
            get
            {
                return stacVersion;
            }

            set
            {
                stacVersion = value;
            }
        }

        [JsonConverter(typeof(CollectionConverter<StacLink>))]
        [JsonProperty("links")]
        public Collection<StacLink> Links
        {
            get
            {
                if (links == null)
                    links = new Collection<StacLink>();
                return links;
            }
            set
            {
                links = value;
            }
        }

        [JsonProperty("assets")]
        public Dictionary<string, StacAsset> Assets
        {
            get
            {
                if (assets == null)
                    assets = new Dictionary<string, StacAsset>();
                return assets;
            }
        }

        [JsonProperty("collection")]
        public string Collection
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
            }
        }

        [JsonIgnore]
        public Itenso.TimePeriod.ITimePeriod DateTime
        {
            get
            {
                if (Properties.ContainsKey("datetime"))
                {
                    if (Properties["datetime"] is DateTime)
                        return new Itenso.TimePeriod.TimeInterval((DateTime)Properties["datetime"]);
                    else
                    {
                        try
                        {
                            return new Itenso.TimePeriod.TimeInterval(System.DateTime.Parse(Properties["datetime"].ToString()));
                        }
                        catch (Exception e)
                        {
                            throw new FormatException(string.Format("{0} is not a valid"), e);
                        }
                    }
                }
                if (Properties.ContainsKey("start_datetime") && Properties.ContainsKey("end_datetime"))
                {
                    if (Properties["start_datetime"] is DateTime && Properties["end_datetime"] is DateTime)
                        return new Itenso.TimePeriod.TimeInterval((DateTime)Properties["start_datetime"],
                                                                    (DateTime)Properties["end_datetime"]);
                    else
                    {
                        try
                        {
                            return new Itenso.TimePeriod.TimeInterval(System.DateTime.Parse(Properties["start_datetime"].ToString()),
                                                                        System.DateTime.Parse(Properties["end_datetime"].ToString()));
                        }
                        catch (Exception e)
                        {
                            throw new FormatException(string.Format("{0} is not a valid"), e);
                        }
                    }
                }

                return null;
            }
        }
    }
}

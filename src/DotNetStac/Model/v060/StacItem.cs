using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DotNetStac;
using Stac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Extensions;

namespace Stac.Model.v060
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    internal class StacItem060 : GeoJSON.Net.Feature.Feature, IStacObject, IStacItem
    {
        private Collection<StacLink> links;

        private Dictionary<string, StacAsset> assets;

        private string stacVersion = StacVersionList.Current;

        private string collection;

        [JsonConstructor]
        public StacItem060(IGeometryObject geometry, IDictionary<string, object> properties = null, string id = null) : base(geometry, properties, id)
        { }

        public StacItem060(IGeometryObject geometry, object properties, string id = null) : base(geometry, properties, id)
        { }

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
        public IDictionary<string, StacAsset> Assets
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

        [JsonIgnore]
        public Uri Uri => throw new NotImplementedException();

        [JsonIgnore]
        public string StacVersion => StacVersionList.V060;

        [JsonIgnore]
        public StacExtensions StacExtensions => null;

        public bool IsCatalog => true;

        public IStacObject Upgrade()
        {
            var item = new v070.StacItem070(this.Geometry,
                                            this.Properties,
                                            this.Id);
            item.Links = this.links;
            foreach (var asset in this.Assets)
                item.Assets.Add(asset.Key, asset.Value);
            item.Collection = this.Collection;
            item.BoundingBoxes = this.BoundingBoxes;
            return item;
        }

    
    }
}

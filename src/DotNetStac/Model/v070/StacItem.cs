using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stac;
using Stac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Extensions;
using System.Runtime.Serialization;
using System.Linq;

namespace Stac.Model.v070
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    internal class StacItem070 : GeoJSON.Net.Feature.Feature, IStacObject, IStacItem, IInternalStacObject
    {
        private Collection<StacLink> links;

        private Dictionary<string, StacAsset> assets;

        private string collection;
        private Uri sourceUri;
        private StacExtensions extensions;

        [JsonConstructor]
        public StacItem070(IGeometryObject geometry, IDictionary<string, object> properties = null, string id = null) : base(geometry, properties, id)
        { }

        public StacItem070(IGeometryObject geometry, object properties, string id = null) : base(geometry, properties, id)
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

        public Uri Uri { get => sourceUri; set => sourceUri = value; }

        public string StacVersion { get => StacVersionList.V070; set { } }

        [JsonProperty("stac_extensions")]
        public string[] StacExtensionsStrings { get; set; }

        [JsonIgnore]
        public StacExtensions StacExtensions
        {
            get
            {
                if (extensions == null)
                    extensions = new StacExtensions();
                return extensions;
            }
            set
            {
                extensions = value;
                extensions.InitStacObject(this);
            }
        }

        public bool IsCatalog => true;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach (StacLink link in Links)
            {
                link.Parent = this;
            }
            StacExtensions = StacExtensionsFactory.Default.LoadStacExtensions(StacExtensionsStrings, this);
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if (BoundingBoxes == null)
                BoundingBoxes = this.GetBoundingBoxFromGeometryExtent();
            StacExtensionsStrings = StacExtensionsStrings.Concat(StacExtensions.Keys).Distinct().ToArray();
        }

        public IStacObject Upgrade()
        {
            var item = new Item.StacItem(this.Geometry,
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

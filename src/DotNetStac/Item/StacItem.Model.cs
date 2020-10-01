using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Stac;
using Stac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Extensions;
using Stac.Model;
using System.Linq;

namespace Stac.Item
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public partial class StacItem : GeoJSON.Net.Feature.Feature, IStacObject, IInternalStacObject, IStacItem
    {
        private Collection<StacLink> links;

        private Dictionary<string, StacAsset> assets;

        private string stacVersion = StacVersionList.Current;

        private StacExtensions extensions;
        private string collection;

        private Uri sourceUri;

        [JsonConstructor]
        public StacItem(IGeometryObject geometry, IDictionary<string, object> properties = null, string id = null) : base(geometry, properties, id)
        { 
        }

        public StacItem(IGeometryObject geometry, object properties, string id = null) : base(geometry, properties, id)
        { 
        }

        [JsonProperty("stac_extensions")]
        [JsonConverter(typeof(StacExtensionConverter))]
        public StacExtensions StacExtensions
        {
            get
            {
                if (extensions == null){
                    extensions = new StacExtensions();
                    extensions.InitStacObject(this);
                }
                return extensions;
            }
            set
            {
                extensions = value;
                extensions.InitStacObject(this);
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
                                    catch (Exception e1)
                                    {
                                        throw new FormatException(string.Format("start_datetime or end_datetime {0} is not a valid"), e1);
                                    }
                                }
                            }
                            throw new FormatException(string.Format("datetime {0} is not a valid"), e);
                        }
                    }
                }


                return null;
            }
        }

        public Uri Uri { get => sourceUri; set => sourceUri = value; }

        public IStacObject Upgrade()
        {
            return this;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach (StacLink link in Links)
            {
                link.Parent = this;
            }
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if ( BoundingBoxes == null )
                SetBoundingBoxOnGeometryExtent();
        }

        [JsonIgnore]
        public bool IsCatalog => false;
    }
}

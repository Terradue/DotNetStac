using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Stac;
using Stac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Extensions;
using System.Linq;
using System.Net.Mime;
using Newtonsoft.Json.Linq;

namespace Stac
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacItem : GeoJSON.Net.Feature.Feature, IStacObject, IInternalStacObject, IStacItem
    {
        public const string MEDIATYPE = "application/json; profile=stac-item";
        public readonly static ContentType ITEM_MEDIATYPE = new ContentType(MEDIATYPE);

        private Collection<StacLink> links;

        private Dictionary<string, StacAsset> assets;

        private string stacVersion = StacVersionList.Current;

        private string collection;

        private Uri sourceUri;

        private string[] stacExtensionsStrings = new string[0];

        [JsonConstructor]
        public StacItem(IGeometryObject geometry, IDictionary<string, object> properties = null, string id = null) : base(Preconditions.CheckNotNull(geometry), properties, id)
        {
            if (geometry == null) throw new ArgumentNullException("geometry");
        }

        public StacItem(IGeometryObject geometry, object properties, string id = null) : base(Preconditions.CheckNotNull(geometry), properties, id)
        {
            if (geometry == null) throw new ArgumentNullException("geometry");
            if (properties == null) throw new ArgumentNullException("properties");
        }

        public StacItem(StacItem stacItem) : this(Preconditions.CheckNotNull(stacItem).Geometry, 
                                                  new Dictionary<string, object>(Preconditions.CheckNotNull(stacItem).Properties), 
                                                  Preconditions.CheckNotNull(stacItem).Id)
        {
            this.stacExtensionsStrings = stacItem.stacExtensionsStrings;
            this.stacVersion = stacItem.stacVersion;
            this.links = new Collection<StacLink>(stacItem.Links);
            this.assets = new Dictionary<string, StacAsset>(stacItem.Assets);
            this.collection = stacItem.collection;
            this.sourceUri = stacItem.sourceUri;
            this.extensions = new StacExtensions(stacItem.StacExtensions);
        }

        private static IStacItem LoadStacItem(JToken jsonRoot)
        {
            Type itemType = null;
            if (jsonRoot["stac_version"] == null)
            {
                throw new InvalidStacDataException("The document is not a valid STAC document. No 'stac_version' property found");
            }

            try
            {
                itemType = Stac.Model.SchemaDictionary.GetItemTypeFromVersion(jsonRoot["stac_version"].Value<string>());
            }
            catch (KeyNotFoundException)
            {
                throw new NotSupportedException(string.Format("The document has a non supprted version: '{0}'.", jsonRoot["stac_version"].Value<string>()));
            }

            return (IStacItem)jsonRoot.ToObject(itemType);
        }

        [JsonProperty("stac_extensions")]
        public string[] StacExtensionsStrings { get => stacExtensionsStrings; set => stacExtensionsStrings = value; }


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



        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach (StacLink link in Links)
            {
                link.Parent = this;
            }
            StacExtensions = StacExtensionsFactory.Default.LoadStacExtensions(StacExtensionsStrings.ToList(), this);
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if (BoundingBoxes == null)
                BoundingBoxes = this.GetBoundingBoxFromGeometryExtent();
            StacExtensionsStrings = StacExtensionsStrings.Concat(StacExtensions.Keys).Distinct().ToArray();
        }

        [JsonIgnore]
        public ContentType MediaType => ITEM_MEDIATYPE;

        [JsonIgnore]
        public Uri Uri
        {
            get
            {
                if (sourceUri == null)
                {
                    return new Uri(Id + ".json", UriKind.Relative);
                }
                return sourceUri;
            }
            set { sourceUri = value; }
        }

        public IStacObject Upgrade()
        {
            return this;
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
                return Itenso.TimePeriod.TimeInterval.Anytime;
            }
            set
            {
                // remove previous values
                Properties.Remove("datetime");
                Properties.Remove("start_datetime");
                Properties.Remove("end_datetime");

                // datetime, start_datetime, end_datetime
                if (value.IsAnytime)
                {
                    Properties.Add("datetime", null);
                }

                if (value.IsMoment)
                {
                    Properties.Add("datetime", value.Start);
                }
                else
                {
                    Properties.Add("datetime", value.Start);
                    Properties.Add("start_datetime", value.Start);
                    Properties.Add("end_datetime", value.Start);
                }
            }
        }

        private StacExtensions extensions;

        [JsonIgnore]
        public StacExtensions StacExtensions
        {
            get
            {
                if (extensions == null)
                {
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

        [JsonIgnore]
        public string Title
        {
            get => this.GetProperty<string>("title");
            set => this.SetProperty("title", value);
        }

        [JsonIgnore]
        public string Description
        {
            get => this.GetProperty<string>("description");
            set => this.SetProperty("description", value);
        }

        [JsonIgnore]
        public DateTime Created
        {
            get => this.GetProperty<DateTime>("created");
            set => this.SetProperty("created", value);
        }

        [JsonIgnore]
        public DateTime Updated
        {
            get => this.GetProperty<DateTime>("updated");
            set => this.SetProperty("updated", value);
        }

    }
}

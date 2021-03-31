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
using Semver;

namespace Stac
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacItem : GeoJSON.Net.Feature.Feature, IStacObject
    {
        public const string MEDIATYPE = "application/json; profile=stac-item";
        public readonly static ContentType ITEM_MEDIATYPE = new ContentType(MEDIATYPE);

        [JsonConstructor]
        public StacItem(string id,
                        IGeometryObject geometry,
                        IDictionary<string, object> properties = null) :
                        base(Preconditions.CheckNotNull(geometry, "geometry"), properties, id)
        {
            Preconditions.CheckNotNull(id, "id");
            StacExtensions = new Collection<string>();
            StacVersion = Versions.StacVersionList.Current;
            Links = new Collection<StacLink>();
            Assets = new Dictionary<string, StacAsset>();
        }

        public StacItem(StacItem stacItem) : base(Preconditions.CheckNotNull(stacItem, "geometry").Geometry,
                                                  new Dictionary<string, object>(Preconditions.CheckNotNull(stacItem).Properties),
                                                  Preconditions.CheckNotNull(stacItem, "id").Id)
        {
            this.StacExtensions = stacItem.StacExtensions;
            this.StacVersion = stacItem.StacVersion;
            this.Links = new Collection<StacLink>(stacItem.Links);
            this.Assets = new Dictionary<string, StacAsset>(stacItem.Assets);
            this.Collection = stacItem.Collection;
        }

        # region IStacObject

        /// <summary>
        /// The STAC version the Item implements
        /// </summary>
        /// <value></value>
        [JsonProperty("stac_version")]
        [JsonConverter(typeof(SemVersionConverter))]
        public SemVersion StacVersion { get; set; }

        /// <summary>
        /// A list of extension identifiers the Item implements
        /// </summary>
        /// <value></value>
        [JsonProperty("stac_extensions")]
        public Collection<string> StacExtensions { get; private set; }

        /// <summary>
        /// A list of references to other documents.
        /// </summary>
        /// <value></value>
        [JsonConverter(typeof(CollectionConverter<StacLink>))]
        [JsonProperty("links")]
        public Collection<StacLink> Links
        {
            get; private set;
        }

        [JsonIgnore]
        public ContentType MediaType => ITEM_MEDIATYPE;

        # endregion IStacObject

        [JsonProperty("assets")]
        public IDictionary<string, StacAsset> Assets { get; private set; }

        [JsonProperty("collection")]
        public string Collection { get; internal set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach (StacLink link in Links)
            {
                link.Parent = this;
            }
            foreach (StacAsset asset in Assets.Values)
            {
                asset.ParentStacItem = this;
            }
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if (BoundingBoxes == null)
                BoundingBoxes = this.GetBoundingBoxFromGeometryExtent();
        }

        public bool ShouldSerializeStacExtensions()
        {
            // don't serialize the Manager property if an employee is their own manager
            return StacExtensions.Count > 0;
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

        [JsonIgnore]
        public IStacObject StacObjectContainer => this;
    }
}

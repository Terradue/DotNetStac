using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Stac;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Extensions;

namespace Stac.Item
{
    public partial class StacItem : IStacItem, IStacObject
    {

        public static async Task<IStacItem> LoadUri(Uri uri)
        {
            var catalog = await StacFactory.LoadUriAsync(uri);
            if (catalog is IStacItem)
                return (IStacItem)catalog;
            throw new InvalidOperationException(string.Format("This is not a STAC item {0}", catalog.Uri));
        }

        public static async Task<IStacItem> LoadStacLink(StacLink link)
        {
            var catalog = await StacFactory.LoadStacLink(link);
            if (catalog is IStacItem)
                return (IStacItem)catalog;
            throw new InvalidOperationException(string.Format("This is not a STAC item {0}", catalog.Uri));
        }

        public static IStacItem LoadJToken(JToken jsonRoot, Uri uri)
        {
            IStacItem item = LoadStacItem(jsonRoot);
            ((IInternalStacObject)item).Uri = uri;
            return item;
        }

        private static IStacItem LoadStacItem(JToken jsonRoot)
        {
            Type itemType = null;
            if (jsonRoot["stac_version"] == null)
            {
                throw new InvalidDataException("The document is not a STAC document. No 'stac_version' property found");
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

        [JsonIgnore]
        public bool IsCatalog => false;

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
                return null;
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

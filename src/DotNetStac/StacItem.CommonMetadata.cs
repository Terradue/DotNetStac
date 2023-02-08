// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacItem.CommonMetadata.cs

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Stac
{
    /// <summary>
    /// This class allows accessing commonly used fields in STAC Item
    /// They are often used in STAC Item properties, but can also be used in other places, e.g. an Item Asset or Collection Asset.
    /// <seealso href="https://github.com/radiantearth/stac-spec/blob/dev/item-spec/common-metadata.md">STAC Common Metadata</seealso>
    /// </summary>
    public partial class StacItem : GeoJSON.Net.Feature.Feature, IStacObject
    {
        /// <inheritdoc/>
        public string Title
        {
            get => this.GetProperty<string>("title");
            set => this.SetProperty("title", value);
        }

        /// <summary>
        /// Gets or sets the description of the Item.
        /// </summary>
        [JsonProperty("__description", Required = Required.Default)]
        [JsonIgnore]
        public string Description
        {
            get => this.GetProperty<string>("description");
            set => this.SetProperty("description", value);
        }

        /// <summary>
        /// Gets or sets the license of the Item.
        /// </summary>
        public string License
        {
            get => this.GetProperty<string>("license");
            set => this.SetProperty("license", value);
        }

        /// <summary>
        /// Gets a list of providers, which may include all organizations capturing or processing the data or the hosting provider.
        /// Providers should be listed in chronological order with the most recent provider being the last element of the list.
        /// </summary>
        public Collection<StacProvider> Providers => this.GetObservableCollectionProperty<StacProvider>("providers");

        /// <summary>
        /// Gets or sets the platform the data was acquired from.
        /// </summary>
        public string Platform
        {
            get => this.GetProperty<string>("platform");
            set => this.SetProperty("platform", value);
        }

        /// <summary>
        /// Gets or sets the instruments used to acquire the data.
        /// </summary>
        public IEnumerable<string> Instruments
        {
            get => this.GetProperty<string[]>("instruments");
            set => this.SetProperty("instruments", value);
        }

        /// <summary>
        /// Gets or sets the constellation the data was acquired from.
        /// </summary>
        public string Constellation
        {
            get => this.GetProperty<string>("constellation");
            set => this.SetProperty("constellation", value);
        }

        /// <summary>
        /// Gets or sets the mission the data was acquired from.
        /// </summary>
        public string Mission
        {
            get => this.GetProperty<string>("mission");
            set => this.SetProperty("mission", value);
        }

        /// <summary>
        /// Gets or sets the name of the satellite the data was acquired from.
        /// </summary>
        public double? Gsd
        {
            get => this.GetProperty<double>("gsd");
            set
            {
                if (value == 0)
                {
                    this.RemoveProperty("gsd");
                }
                else
                {
                    this.SetProperty("gsd", value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the created date and time of the Item.
        /// </summary>
        public DateTime Created
        {
            get => this.GetProperty<DateTime>("created");
            set => this.SetProperty("created", value);
        }

        /// <summary>
        /// Gets or sets the updated date and time of the Item.
        /// </summary>
        public DateTime Updated
        {
            get => this.GetProperty<DateTime>("updated");
            set => this.SetProperty("updated", value);
        }

        /// <summary>
        /// Gets or sets the date and time the data was acquired.
        /// </summary>
        public Itenso.TimePeriod.ITimePeriod DateTime
        {
            get
            {
                if (this.Properties.ContainsKey("datetime"))
                {
                    if (this.Properties["datetime"] is DateTime?)
                    {
                        return new Itenso.TimePeriod.TimeInterval((DateTime)this.Properties["datetime"]);
                    }
                    else
                    {
                        try
                        {
                            return new Itenso.TimePeriod.TimeInterval(System.DateTime.Parse(this.Properties["datetime"].ToString()));
                        }
                        catch (NullReferenceException nre)
                        {
                            if (!this.Properties.ContainsKey("datetime") || (!this.Properties.ContainsKey("start_datetime") && !this.Properties.ContainsKey("end_datetime")))
                            {
                                throw new FormatException("One of datetime, start_datetime and/or end_datetime are required", nre);
                            }
                        }

                        if (this.Properties.ContainsKey("start_datetime") && this.Properties.ContainsKey("end_datetime"))
                        {
                            if (this.Properties["start_datetime"] is DateTime? && this.Properties["end_datetime"] is DateTime?)
                            {
                                return new Itenso.TimePeriod.TimeInterval(
                                    (DateTime)this.Properties["start_datetime"],
                                    (DateTime)this.Properties["end_datetime"]);
                            }

                            throw new FormatException(string.Format("start_datetime and/or end_datetime are not a valid"));
                        }
                    }
                }

                return Itenso.TimePeriod.TimeInterval.Anytime;
            }

            set
            {
                // datetime, start_datetime, end_datetime
                if (value == null)
                {
                    this.RemoveProperty("start_datetime");
                    this.RemoveProperty("end_datetime");
                    this.SetProperty("datetime", null);
                    return;
                }

                // datetime, start_datetime, end_datetime
                if (value.IsAnytime)
                {
                    this.RemoveProperty("start_datetime");
                    this.RemoveProperty("end_datetime");
                    this.SetProperty("datetime", null);
                }

                if (value.IsMoment)
                {
                    this.RemoveProperty("start_datetime");
                    this.RemoveProperty("end_datetime");
                    this.SetProperty("datetime", value.Start);
                }
                else
                {
                    this.SetProperty("datetime", value.Start);
                    this.SetProperty("start_datetime", value.Start);
                    this.SetProperty("end_datetime", value.End);
                }
            }
        }
    }
}

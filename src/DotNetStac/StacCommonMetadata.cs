using System;
using System.Collections.Generic;
using System.Linq;

namespace Stac
{
    /// <summary>
    /// This class allows accessing commonly used fields in STAC. 
    /// They are often used in STAC Item properties, but can also be used in other places, e.g. an Item Asset or Collection Asset.
    /// <seealso href="https://github.com/radiantearth/stac-spec/blob/dev/item-spec/common-metadata.md">STAC Common Metadata</seealso>
    /// </summary>
    public class StacCommonMetadata
    {
        private IStacPropertiesContainer stacPropertiesContainer;

        public StacCommonMetadata(IStacPropertiesContainer stacPropertiesContainer)
        {
            this.stacPropertiesContainer = stacPropertiesContainer;
        }

        public string Title
        {
            get
            {
                return stacPropertiesContainer.GetProperty<string>("title");
            }
            set
            {
                stacPropertiesContainer.SetProperty("title", value);
            }
        }

        public string Description
        {
            get
            {
                return stacPropertiesContainer.GetProperty<string>("description");
            }
            set
            {
                stacPropertiesContainer.SetProperty("description", value);
            }
        }

        public string License
        {
            get
            {
                return stacPropertiesContainer.GetProperty<string>("license");
            }
            set
            {
                stacPropertiesContainer.SetProperty("license", value);
            }
        }

        public string Platform
        {
            get
            {
                return stacPropertiesContainer.GetProperty<string>("platform");
            }
            set
            {
                stacPropertiesContainer.SetProperty("platform", value);
            }
        }

        public IEnumerable<string> Instruments
        {
            get
            {
                return stacPropertiesContainer.GetProperty<IEnumerable<string>>("instruments");
            }
            set
            {
                stacPropertiesContainer.SetProperty("instruments", value);
            }
        }

        public string Constellation
        {
            get
            {
                return stacPropertiesContainer.GetProperty<string>("constellation");
            }
            set
            {
                stacPropertiesContainer.SetProperty("constellation", value);
            }
        }

        public string Mission
        {
            get
            {
                return stacPropertiesContainer.GetProperty<string>("mission");
            }
            set
            {
                stacPropertiesContainer.SetProperty("mission", value);
            }
        }

        public int Gsd
        {
            get
            {
                return stacPropertiesContainer.GetProperty<int>("gsd");
            }
            set
            {
                stacPropertiesContainer.SetProperty("gsd", value);
            }
        }

        public DateTime Created
        {
            get => stacPropertiesContainer.GetProperty<DateTime>("created");
            set => stacPropertiesContainer.SetProperty("created", value);
        }

        public DateTime Updated
        {
            get => stacPropertiesContainer.GetProperty<DateTime>("updated");
            set => stacPropertiesContainer.SetProperty("updated", value);
        }

        public Itenso.TimePeriod.ITimePeriod DateTime
        {
            get
            {
                if (stacPropertiesContainer.Properties.ContainsKey("datetime"))
                {
                    if (stacPropertiesContainer.Properties["datetime"] is DateTime)
                        return new Itenso.TimePeriod.TimeInterval((DateTime)stacPropertiesContainer.Properties["datetime"]);
                    else
                    {
                        try
                        {
                            return new Itenso.TimePeriod.TimeInterval(System.DateTime.Parse(stacPropertiesContainer.Properties["datetime"].ToString()));
                        }
                        catch (Exception e)
                        {
                            if (stacPropertiesContainer.Properties.ContainsKey("start_datetime") && stacPropertiesContainer.Properties.ContainsKey("end_datetime"))
                            {
                                if (stacPropertiesContainer.Properties["start_datetime"] is DateTime && stacPropertiesContainer.Properties["end_datetime"] is DateTime)
                                    return new Itenso.TimePeriod.TimeInterval((DateTime)stacPropertiesContainer.Properties["start_datetime"],
                                                                                (DateTime)stacPropertiesContainer.Properties["end_datetime"]);
                                throw new FormatException(string.Format("start_datetime and/or end_datetime are not a valid: {0}", e.Message), e);
                            } 
                            throw new FormatException(string.Format("datetime is not a valid", e.Message), e);
                        }
                    }
                }
                return Itenso.TimePeriod.TimeInterval.Anytime;
            }
            set
            {
                // remove previous values
                stacPropertiesContainer.Properties.Remove("datetime");
                stacPropertiesContainer.Properties.Remove("start_datetime");
                stacPropertiesContainer.Properties.Remove("end_datetime");

                // datetime, start_datetime, end_datetime
                if (value.IsAnytime)
                {
                    stacPropertiesContainer.Properties.Add("datetime", null);
                }

                if (value.IsMoment)
                {
                    stacPropertiesContainer.Properties.Add("datetime", value.Start);
                }
                else
                {
                    stacPropertiesContainer.Properties.Add("datetime", value.Start);
                    stacPropertiesContainer.Properties.Add("start_datetime", value.Start);
                    stacPropertiesContainer.Properties.Add("end_datetime", value.End);
                }
            }
        }

    }

    public static class StacCommonMetadataExtensions
    {
        public static StacCommonMetadata CommonMetadata(this IStacPropertiesContainer stacPropertiesContainer)
        {
            return new StacCommonMetadata(stacPropertiesContainer);
        }
    }
}
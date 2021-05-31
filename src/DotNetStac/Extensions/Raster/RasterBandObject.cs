using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Stac.Common;

namespace Stac.Extensions.Raster
{
    /// <summary>
    /// Represents the <seealso href="https://github.com/stac-extensions/raster/#raster-band-object">Raster Band Object</seealso>
    /// of the Raster extension
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class RasterBand : IStacPropertiesContainer
    {

        IDictionary<string, object> properties;
        private double? nodata;
        private RasterSampling? sampling;
        private DataType? dataType;
        private string unit;
        private double? scale;
        private double? offset;

        /// <summary>
        /// Initialize a new Raster Band Object
        /// </summary>
        public RasterBand()
        {
            properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Pixel values used to identify pixels that are nodata in the assets.
        /// </summary>
        [JsonProperty("nodata")]
        public double? Nodata { get => nodata; set => nodata = value; }

        /// <summary>
        /// One of area or point. Indicates whether a pixel value should be assumed to represent 
        /// a sampling over the region of the pixel or a point sample at the center of the pixel.
        /// </summary>
        [JsonProperty("sampling")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RasterSampling? Sampling { get => sampling; set => sampling = value; }

        /// <summary>
        /// The data type of the band.
        /// </summary>
        /// <value></value>
        [JsonProperty("data_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DataType? DataType { get => dataType; set => dataType = value; }

        /// <summary>
        /// The actual number of bits used for this band.
        /// </summary>
        /// <value></value>
        [JsonProperty("bits_per_sample")]
        public int? BitsPerSample { get; set; }

        /// <summary>
        /// Average spatial resolution (in meters) of the pixels in the band.
        /// </summary>
        /// <value></value>
        [JsonProperty("spatial_resolution")]
        public double? SpatialResolution { get; set; }

        /// <summary>
        /// Statistics of all the pixels in the band
        /// </summary>
        /// <value></value>
        [JsonProperty("statistics")]
        public Statistics Statistics { get; set; }

        /// <summary>
        /// Unit denomination of the pixel value.
        /// </summary>
        [JsonProperty("unit")]
        public string Unit { get => unit; set => unit = value; }

        /// <summary>
        /// multiplicator factor of the pixel value to transform into the value (i.e. translate digital number to reflectance).
        /// </summary>
        [JsonProperty("scale")]
        public double? Scale { get => scale; set => scale = value; }

        /// <summary>
        /// number to be added to the pixel value (after scaling) to transform into the value (i.e. translate digital number to reflectance).
        /// </summary>
        [JsonProperty("offset")]
        public double? Offset { get => offset; set => offset = value; }

        /// <summary>
        /// Histogram distribution information of the pixels values in the band
        /// </summary>
        /// <value></value>
        [JsonProperty("histogram")]
        public RasterHistogram Histogram { get; set; }

        /// <summary>
        /// Additional fields
        /// </summary>
        /// <value></value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get => properties; set => properties = value; }

        [JsonIgnore]
        public IStacObject StacObjectContainer => null;
    }
}

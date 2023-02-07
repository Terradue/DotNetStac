// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: RasterBandObject.cs

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
        private IDictionary<string, object> _properties;
        private double? _nodata;
        private RasterSampling? _sampling;
        private DataType? _dataType;
        private string _unit;
        private double? _scale;
        private double? _offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterBand"/> class.
        /// </summary>
        public RasterBand()
        {
            this._properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets pixel values used to identify pixels that are nodata in the assets.
        /// </summary>
        /// <value>
        /// Pixel values used to identify pixels that are nodata in the assets.
        /// </value>
        [JsonProperty("nodata")]
        public double? Nodata { get => this._nodata; set => this._nodata = value; }

        /// <summary>
        /// Gets or sets one of area or point. Indicates whether a pixel value should be assumed to represent 
        /// a sampling over the region of the pixel or a point sample at the center of the pixel.
        /// </summary>
        /// <value>
        /// One of area or point. Indicates whether a pixel value should be assumed to represent 
        /// a sampling over the region of the pixel or a point sample at the center of the pixel.
        /// </value>
        [JsonProperty("sampling")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RasterSampling? Sampling { get => this._sampling; set => this._sampling = value; }

        /// <summary>
        /// Gets or sets the data type of the band.
        /// </summary>
        /// <value>
        /// The data type of the band.
        /// </value>
        [JsonProperty("data_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DataType? DataType { get => this._dataType; set => this._dataType = value; }

        /// <summary>
        /// Gets or sets the actual number of bits used for this band.
        /// </summary>
        /// <value>
        /// The actual number of bits used for this band.
        /// </value>
        [JsonProperty("bits_per_sample")]
        public int? BitsPerSample { get; set; }

        /// <summary>
        /// Gets or sets average spatial resolution (in meters) of the pixels in the band.
        /// </summary>
        /// <value>
        /// Average spatial resolution (in meters) of the pixels in the band.
        /// </value>
        [JsonProperty("spatial_resolution")]
        public double? SpatialResolution { get; set; }

        /// <summary>
        /// Gets or sets statistics of all the pixels in the band
        /// </summary>
        /// <value>
        /// Statistics of all the pixels in the band
        /// </value>
        [JsonProperty("statistics")]
        public Statistics Statistics { get; set; }

        /// <summary>
        /// Gets or sets unit denomination of the pixel value.
        /// </summary>
        /// <value>
        /// Unit denomination of the pixel value.
        /// </value>
        [JsonProperty("unit")]
        public string Unit { get => this._unit; set => this._unit = value; }

        /// <summary>
        /// Gets or sets multiplicator factor of the pixel value to transform into the value (i.e. translate digital number to reflectance).
        /// </summary>
        /// <value>
        /// multiplicator factor of the pixel value to transform into the value (i.e. translate digital number to reflectance).
        /// </value>
        [JsonProperty("scale")]
        public double? Scale { get => this._scale; set => this._scale = value; }

        /// <summary>
        /// Gets or sets number to be added to the pixel value (after scaling) to transform into the value (i.e. translate digital number to reflectance).
        /// </summary>
        /// <value>
        /// number to be added to the pixel value (after scaling) to transform into the value (i.e. translate digital number to reflectance).
        /// </value>
        [JsonProperty("offset")]
        public double? Offset { get => this._offset; set => this._offset = value; }

        /// <summary>
        /// Gets or sets histogram distribution information of the pixels values in the band
        /// </summary>
        /// <value>
        /// Histogram distribution information of the pixels values in the band
        /// </value>
        [JsonProperty("histogram")]
        public RasterHistogram Histogram { get; set; }

        /// <summary>
        /// Gets or sets additional fields
        /// </summary>
        /// <value>
        /// Additional fields
        /// </value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get => this._properties; set => this._properties = value; }

        /// <inheritdoc/>
        [JsonIgnore]
        public IStacObject StacObjectContainer => null;
    }
}

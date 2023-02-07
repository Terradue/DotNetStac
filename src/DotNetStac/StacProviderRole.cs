// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacProviderRole.cs

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac
{
    /// <summary>
    /// Possible roles of a provider.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StacProviderRole
    {
        /// The organization that is licensing the dataset under the license specified in the Collection's <see cref="StacCollection.License" /> field.
        licensor,

        // The producer of the data is the provider that initially captured and processed the source data, e.g. ESA for Sentinel-2 data.
        producer,

        // A processor is any provider who processed data to a derived product.
        processor,

        //  The host is the actual provider offering the data on their storage. There should be no more than one host, specified as last element of the list.
        host,
    }
}

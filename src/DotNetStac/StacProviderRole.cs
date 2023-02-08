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
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented
        /// <summary>
        // The organization that is licensing the dataset under the license specified in the Collection's <see cref="StacCollection.License" /> field.
        // </summary>
        licensor,

        /// <summary>
        /// The producer of the data is the provider that initially captured and processed the source data, e.g. ESA for Sentinel-2 data.
        /// </summary>
        producer,

        /// <summary>
        /// A processor is any provider who processed data to a derived product.
        /// </summary>
        processor,

        /// <summary>
        /// The host is the actual provider offering the data on their storage. There should be no more than one host, specified as last element of the list.
        /// </summary>
        host,
#pragma warning restore SA1602 // EnumerationItemsMustBeDocumented
#pragma warning restore SA1300 // Element should begin with upper-case letter
    }
}

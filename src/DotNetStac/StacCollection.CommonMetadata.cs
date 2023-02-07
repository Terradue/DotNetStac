// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacCollection.CommonMetadata.cs

using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Stac
{
    /// <summary>
    /// This class allows accessing commonly used fields in STAC Item 
    /// They are often used in STAC Item properties, but can also be used in other places, e.g. an Item Asset or Collection Asset.
    /// <seealso href="https://github.com/radiantearth/stac-spec/blob/dev/item-spec/common-metadata.md">STAC Common Metadata</seealso>
    /// </summary>
    public partial class StacCollection : IStacObject, IStacParent, IStacCatalog, ICloneable
    {

        /// <summary>
        /// Gets or sets a short descriptive one-line title for the Collection.
        /// </summary>
        /// <value>
        /// A short descriptive one-line title for the Collection.
        /// </value>
        [JsonProperty("__title", Required = Required.Default)]
        [JsonIgnore]
        public string Title
        {
            get => this.GetProperty<string>("title");
            set => this.SetProperty("title", value);
        }

        /// <summary>
        /// Gets or sets detailed multi-line description to fully explain the Collection. CommonMark 0.29 syntax MAY be used for rich text representation.
        /// </summary>
        /// <value>
        /// Detailed multi-line description to fully explain the Collection. CommonMark 0.29 syntax MAY be used for rich text representation.
        /// </value>
        [JsonProperty("__description", Required = Required.Default)]
        [JsonIgnore]
        public string Description
        {
            get => this.GetProperty<string>("description");
            set => this.SetProperty("description", value);
        }

        /// <summary>
        /// Gets or sets collection's license(s), either a SPDX License identifier, various if multiple licenses apply or proprietary for all other cases.
        /// </summary>
        /// <value>
        /// Collection's license(s), either a SPDX License identifier, various if multiple licenses apply or proprietary for all other cases.
        /// </value>
        public string License
        {
            get => this.GetProperty<string>("license");
            set => this.SetProperty("license", value);
        }

        /// <summary>
        /// Gets a list of providers, which may include all organizations capturing or processing the data or the hosting provider. 
        /// Providers should be listed in chronological order with the most recent provider being the last element of the list.
        /// </summary>
        /// <value>
        /// A list of providers, which may include all organizations capturing or processing the data or the hosting provider. 
        /// Providers should be listed in chronological order with the most recent provider being the last element of the list.
        /// </value>
        [JsonProperty("__providers", Required = Required.Default)]
        [JsonIgnore]
        public Collection<StacProvider> Providers => this.GetObservableCollectionProperty<StacProvider>("providers");
    }
}

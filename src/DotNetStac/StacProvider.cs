// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacProvider.cs

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace Stac
{
    /// <summary>
    /// The class provides properties about a provider. 
    /// A provider is any of the organizations that captures or processes the content of the Collection and 
    /// therefore influences the data offered by this Collection. 
    /// May also include information about the final storage provider hosting the data.
    /// <seealso href="https://github.com/radiantearth/stac-spec/blob/dev/collection-spec/collection-spec.md#provider-object">STAC Provider Object</seealso>
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacProvider
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="StacProvider" /> class.
        /// </summary>
        /// <param name="name">The name of the organization or the individual.</param>
        /// <param name="providerRoles">Roles of the provider. Any of <see cref="StacProviderRole" />.</param>
        public StacProvider(string name, IEnumerable<StacProviderRole> providerRoles = null)
        {
            this.Name = name;
            if (providerRoles != null)
                this.Roles = new Collection<StacProviderRole>(providerRoles.ToList());
            else
                this.Roles = new Collection<StacProviderRole>();
        }

        /// <summary>
        /// The name of the organization or the individual.
        /// </summary>
        /// <value>Gets/sets the name</value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Multi-line description to add further provider information such as processing details for processors and producers, 
        /// hosting details for hosts or basic contact information. 
        /// CommonMark 0.29 syntax MAY be used for rich text representation.
        /// </summary>
        /// <value>Gets/sets the description</value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Roles of the provider. Any of <see cref="StacProviderRole" />.
        /// </summary>
        /// <value>Gets the roles collection</value>
        [JsonProperty("roles")]
        public Collection<StacProviderRole> Roles { get; private set; }

        /// <summary>
        /// Homepage on which the provider describes the dataset and publishes contact information.
        /// </summary>
        /// <value>Gets/sets the URL</value>
        [JsonProperty("url")]
        public Uri Uri { get; set; }

#pragma warning disable 1591
        public bool ShouldSerializeRoles()
        {
            // don't serialize the Manager property if an employee is their own manager
            return this.Roles.Count > 0;
        }
    }
}

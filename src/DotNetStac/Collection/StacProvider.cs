using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Collection
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacProvider
    {
        public StacProvider(string name, IEnumerable<StacProviderRole> providerRoles = null)
        {
            this.Name = name;
            if (providerRoles != null)
                Roles = new Collection<StacProviderRole>(providerRoles.ToList());
            else
                Roles = new Collection<StacProviderRole>();
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("roles")]
        public Collection<StacProviderRole> Roles { get; private set; }

        [JsonProperty("url")]
        public Uri Uri { get; set; }

#pragma warning disable 1591
        public bool ShouldSerializeRoles()
        {
            // don't serialize the Manager property if an employee is their own manager
            return Roles.Count > 0;
        }
    }
}
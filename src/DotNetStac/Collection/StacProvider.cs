using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Collection
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacProvider
    {
        private string name;

        private string description;

        private List<StacProviderRole> roles;
        private Uri uri;

        public StacProvider(string name)
        {
            this.name = name;
        }

        [JsonProperty("name")]
        public string Name { get => name; set => name = value; }

        [JsonProperty("description")]
        public string Description { get => description; set => description = value; }

        [JsonProperty("roles")]
        public List<StacProviderRole> Roles { get => roles; set => roles = value; }

        [JsonProperty("url")]
        public Uri Uri { get => uri; set => uri = value; }
    }
}
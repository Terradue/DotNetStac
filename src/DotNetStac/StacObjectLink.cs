using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stac
{
    public class StacObjectLink : StacLink
    {
        public static StacLink CreateItemLink(IStacObject stacObject)
        {
            return new StacObjectLink(stacObject);
        }

        private readonly IStacObject stacObject;

        public StacObjectLink(IStacObject stacObject, IStacObject hostObject = null)
        {
            this.stacObject = stacObject;
            this.hostObject = hostObject;
        }

        [JsonProperty("type")]
        [JsonConverter(typeof(ContentTypeConverter))]
        public override ContentType MediaType
        {
            get => stacObject.MediaType;
            set
            {
                throw new InvalidOperationException("Cannot set MediaType on an STAC Object link");
            }
        }

        [JsonProperty("rel")]
        public override string RelationshipType
        {
            get { return rel; }
            set { rel = value; }
        }

        [JsonProperty("title")]
        public override string Title
        {
            get => stacObject.GetProperty<string>("title");
            set
            {
                throw new InvalidOperationException("Cannot set Title on an STAC Object link");
            }
        }

        [JsonProperty("href")]
        public override Uri Uri
        {
            get => stacObject.Uri;
            set
            {
                throw new InvalidOperationException("Cannot set Uri on an STAC Object link");
            }
        }

        public override Task<IStacObject> LoadAsync()
        {
            return Task<IStacObject>.FromResult(stacObject);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stac
{
    public class StacObjectLink : StacLink
    {
        private readonly IStacObject stacObject;

        internal StacObjectLink(IStacObject stacObject, IStacObject hostObject = null)
        {
            this.stacObject = stacObject;
            this.hostObject = hostObject;
            if ( stacObject is StacItem )
                this.RelationshipType = "item";
            if ( stacObject is StacCatalog || stacObject is StacCollection )
                this.RelationshipType = "child";
        }

        [JsonProperty("type")]
        [JsonConverter(typeof(ContentTypeConverter))]
        public override ContentType ContentType
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

    }
}

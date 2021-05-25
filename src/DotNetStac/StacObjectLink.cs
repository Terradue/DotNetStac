using System;
using System.Net.Mime;
using Newtonsoft.Json;

namespace Stac
{
    public class StacObjectLink : StacLink
    {
        private readonly IStacObject stacObject;

        internal StacObjectLink(IStacObject stacObject, Uri uri)
        {
            this.stacObject = stacObject;
            if ( stacObject is StacItem )
                this.RelationshipType = "item";
            if ( stacObject is StacCatalog || stacObject is StacCollection )
                this.RelationshipType = "child";
            Uri = uri;
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
            get;
            set;
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
            get;
            set;
        }

        [JsonIgnore]
        public IStacObject StacObject => stacObject;
    }
}

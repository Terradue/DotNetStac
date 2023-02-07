﻿// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacObjectLink.cs

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
            if (stacObject is StacItem)
            {
                this.RelationshipType = "item";
            }

            if (stacObject is StacCatalog || stacObject is StacCollection)
            {
                this.RelationshipType = "child";
            }

            this.Uri = uri;
        }

        /// <inheritdoc/>
        [JsonProperty("type")]
        [JsonConverter(typeof(ContentTypeConverter))]
        public override ContentType ContentType
        {
            get => this.stacObject.MediaType;
            set
            {
                throw new InvalidOperationException("Cannot set MediaType on an STAC Object link");
            }
        }

        /// <inheritdoc/>
        [JsonProperty("rel")]
        public override string RelationshipType
        {
            get;
            set;
        }

        /// <inheritdoc/>
        [JsonProperty("title")]
        public override string Title
        {
            get => this.stacObject.Title;
            set
            {
                throw new InvalidOperationException("Cannot set Title on an STAC Object link");
            }
        }

        /// <inheritdoc/>
        [JsonProperty("href")]
        public override Uri Uri
        {
            get;
            set;
        }

        [JsonIgnore]
        public IStacObject StacObject => this.stacObject;
    }
}

// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: FileStacExtension.cs

using System;
using System.Collections.Generic;
using Multiformats.Hash;

namespace Stac.Extensions.File
{
    /// <summary>
    /// Helper class to access the fields defined by the <seealso href="https://github.com/stac-extensions/file">File extension</seealso>
    /// </summary>
    public class FileStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/file/v1.0.0/schema.json";
        private const string ByteOrderField = "file:byte_order";
        private const string ChecksumField = "file:checksum";
        private const string HeaderSizeField = "file:header_size";
        private const string SizeField = "file:size";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        private readonly Dictionary<string, Type> _itemFields;

        internal FileStacExtension(StacAsset stacAsset)
            : base(JsonSchemaUrl, stacAsset)
        {
            this._itemFields = new Dictionary<string, Type>();
            this._itemFields.Add(ByteOrderField, typeof(string));
            this._itemFields.Add(ChecksumField, typeof(string));
            this._itemFields.Add(HeaderSizeField, typeof(string));
            this._itemFields.Add(SizeField, typeof(IDictionary<string, string>));
        }

        /// <summary>
        /// Gets or sets the byte order of integer values in the file. One of big-endian or little-endian.
        /// </summary>
        public ByteOrder ByteOrder
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<ByteOrder>(ByteOrderField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ByteOrderField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the name commonly used to refer to the processing level to make it easier to search for product level across collections or items.
        /// </summary>
        public Multihash Checksum
        {
            get
            {
                return Multihash.Parse(this.StacPropertiesContainer.GetProperty<string>(ChecksumField));
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ChecksumField, value.ToString());
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the name of the facility that produced the data.
        /// </summary>
        public uint? HeaderSize
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<uint?>(HeaderSizeField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(HeaderSizeField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the name of the facility that produced the data.
        /// </summary>
        public ulong? Size
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<ulong?>(SizeField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(SizeField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets potential fields and their types
        /// </summary>
        public override IDictionary<string, Type> ItemFields => this._itemFields;

        /// <summary>
        /// Gets get the STAC asset
        /// </summary>
        public StacAsset StacAsset => this.StacPropertiesContainer as StacAsset;

        /// <inheritdoc/>
        public override IDictionary<string, ISummaryFunction> GetSummaryFunctions()
        {
            Dictionary<string, ISummaryFunction> summaryFunctions = new Dictionary<string, ISummaryFunction>();
            return summaryFunctions;
        }
    }
}

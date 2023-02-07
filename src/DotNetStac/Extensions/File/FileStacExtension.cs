// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: FileStacExtension.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Multiformats.Base;
using Multiformats.Hash;

namespace Stac.Extensions.File
{
    /// <summary>
    /// Helper class to access the fields defined by the <seealso href="https://github.com/stac-extensions/file">File extension</seealso>
    /// </summary>
    public class FileStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        // Extension identifier and schema url
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/file/v1.0.0/schema.json";
        private const string ByteOrderField = "file:byte_order";
        private const string ChecksumField = "file:checksum";
        private const string HeaderSizeField = "file:header_size";
        private const string SizeField = "file:size";
        private readonly Dictionary<string, Type> itemFields;

        internal FileStacExtension(StacAsset stacAsset) : base(JsonSchemaUrl, stacAsset)
        {
            this.itemFields = new Dictionary<string, Type>();
            this.itemFields.Add(ByteOrderField, typeof(string));
            this.itemFields.Add(ChecksumField, typeof(string));
            this.itemFields.Add(HeaderSizeField, typeof(string));
            this.itemFields.Add(SizeField, typeof(IDictionary<string, string>));
        }

        /// <summary>
        /// Gets or sets the byte order of integer values in the file. One of big-endian or little-endian.
        /// </summary>
        /// <value></value>
        public ByteOrder ByteOrder
        {
            get { return this.StacPropertiesContainer.GetProperty<ByteOrder>(ByteOrderField); }
            set { this.StacPropertiesContainer.SetProperty(ByteOrderField, value); this.DeclareStacExtension(); }
        }

        /// <summary>
        /// Gets or sets the name commonly used to refer to the processing level to make it easier to search for product level across collections or items.
        /// </summary>
        /// <value></value>
        public Multihash Checksum
        {
            get { return Multihash.Parse(this.StacPropertiesContainer.GetProperty<string>(ChecksumField)); }
            set { this.StacPropertiesContainer.SetProperty(ChecksumField, value.ToString()); this.DeclareStacExtension(); }
        }

        /// <summary>
        /// Gets or sets the name of the facility that produced the data. 
        /// </summary>
        /// <value></value>
        public uint? HeaderSize
        {
            get { return this.StacPropertiesContainer.GetProperty<uint?>(HeaderSizeField); }
            set { this.StacPropertiesContainer.SetProperty(HeaderSizeField, value); this.DeclareStacExtension(); }
        }

        /// <summary>
        /// Gets or sets the name of the facility that produced the data. 
        /// </summary>
        /// <value></value>
        public ulong? Size
        {
            get { return this.StacPropertiesContainer.GetProperty<ulong?>(SizeField); }
            set { this.StacPropertiesContainer.SetProperty(SizeField, value); this.DeclareStacExtension(); }
        }

        /// <summary>
        /// Gets potential fields and their types
        /// </summary>
        /// <value>
        /// <placeholder>Potential fields and their types</placeholder>
        /// </value>
        public override IDictionary<string, Type> ItemFields => this.itemFields;

        /// <summary>
        /// Gets get the STAC asset
        /// </summary>
        /// <value>
        /// <placeholder>Get the STAC asset</placeholder>
        /// </value>
        public StacAsset StacAsset => this.StacPropertiesContainer as StacAsset;

        /// <inheritdoc/>
        public override IDictionary<string, ISummaryFunction> GetSummaryFunctions()
        {
            Dictionary<string, ISummaryFunction> summaryFunctions = new Dictionary<string, ISummaryFunction>();
            return summaryFunctions;
        }

    }

    /// <summary>
    /// Extension methods for accessing Processing extension
    /// </summary>
    public static class FileStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a EoStacExtension class from a STAC item
        /// </summary>
        public static FileStacExtension FileExtension(this StacAsset stacAsset)
        {
            return new FileStacExtension(stacAsset);
        }

        /// <summary>
        /// Set possibly file extension properties from a FileInfo object:
        /// - size
        /// - checksum
        /// </summary>
        public static async Task SetFileExtensionProperties(this FileStacExtension fileStacExtension,
                                                      FileInfo file,
                                                      HashType hashType = HashType.SHA1,
                                                      MultibaseEncoding encoding = MultibaseEncoding.Base16Lower)
        {
            fileStacExtension.Size = Convert.ToUInt64(file.Length);
            await fileStacExtension.SetFileCheckSum(hashType, encoding, uri => file.OpenRead());
        }

        /// <summary>
        /// Set possibly file extension properties from a FileInfo object:
        /// - size
        /// - checksum
        /// </summary>
        public static async Task SetFileExtensionProperties(this FileStacExtension fileStacExtension,
                                                      Stream stream,
                                                      HashType hashType = HashType.SHA1,
                                                      MultibaseEncoding encoding = MultibaseEncoding.Base16Lower)
        {
            await fileStacExtension.SetFileCheckSum(hashType, encoding, uri => stream);
        }

        /// <summary>
        /// Add the checksum property of the file extension
        /// </summary>
        public static async Task SetFileCheckSum(this FileStacExtension fileStacExtension,
                                                         HashType hashType,
                                                         MultibaseEncoding encoding,
                                                         Func<Uri, Stream> uriStreamer)
        {
            Multihash mh = null;
            using (var stream = uriStreamer(fileStacExtension.StacAsset.Uri))
            {
                byte[] data = null;
                using (var mem = new MemoryStream())
                {
                    await stream.CopyToAsync(mem);
                    data = mem.ToArray();
                    fileStacExtension.Size = Convert.ToUInt64(mem.Length);
                }

                mh = Multihash.Sum(hashType, data);
            }

            fileStacExtension.Checksum = mh;
        }
    }
}

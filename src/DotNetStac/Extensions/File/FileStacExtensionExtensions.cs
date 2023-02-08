// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: FileStacExtensionExtensions.cs

using System;
using System.IO;
using System.Threading.Tasks;
using Multiformats.Base;
using Multiformats.Hash;

namespace Stac.Extensions.File
{
    /// <summary>
    /// Extension methods for accessing Processing extension
    /// </summary>
    public static class FileStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a EoStacExtension class from a STAC item
        /// </summary>
        /// <param name="stacAsset">The STAC asset.</param>
        /// <returns>The EoStacExtension class</returns>
        public static FileStacExtension FileExtension(this StacAsset stacAsset)
        {
            return new FileStacExtension(stacAsset);
        }

        /// <summary>
        /// Set possibly file extension properties from a FileInfo object:
        /// - size
        /// - checksum
        /// </summary>
        /// <param name="fileStacExtension">The file extension.</param>
        /// <param name="file">The file.</param>
        /// <param name="hashType">Type of the hash.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>No object or value is returned by this method when it completes.</returns>
        public static async Task SetFileExtensionProperties(
            this FileStacExtension fileStacExtension,
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
        /// <param name="fileStacExtension">The file extension.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="hashType">Type of the hash.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>No object or value is returned by this method when it completes.</returns>
        public static async Task SetFileExtensionProperties(
            this FileStacExtension fileStacExtension,
            Stream stream,
            HashType hashType = HashType.SHA1,
            MultibaseEncoding encoding = MultibaseEncoding.Base16Lower)
        {
            await fileStacExtension.SetFileCheckSum(hashType, encoding, uri => stream);
        }

        /// <summary>
        /// Add the checksum property of the file extension
        /// </summary>
        /// <param name="fileStacExtension">The file extension.</param>
        /// <param name="hashType">Type of the hash.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="uriStreamer">The URI streamer.</param>
        /// <returns>No object or value is returned by this method when it completes.</returns>
        public static async Task SetFileCheckSum(
            this FileStacExtension fileStacExtension,
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

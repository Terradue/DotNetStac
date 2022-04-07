using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Multiformats.Base;
using Multiformats.Hash;
using Multiformats.Hash.Algorithms;
using Stac.Model;

namespace Stac.Extensions.Storage
{
    /// <summary>
    /// Helper class to access the fields defined by the <seealso href="https://github.com/stac-extensions/storage">Storage extension</seealso>
    /// </summary>
    public class StorageStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        /// Extension identifier and schema url
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/storage/v1.0.0/schema.json";
        private const string PlatformField = "storage:platform";
        private const string RegionField = "storage:region";
        private const string RequesterPaysField = "storage:requester_pays";
        private const string TierField = "storage:tier";
        private readonly Dictionary<string, Type> itemFields;

        internal StorageStacExtension(IStacPropertiesContainer stacObject) : base(JsonSchemaUrl, stacObject)
        {
            itemFields = new Dictionary<string, Type>();
            itemFields.Add(PlatformField, typeof(string));
            itemFields.Add(RegionField, typeof(string));
            itemFields.Add(RequesterPaysField, typeof(bool));
            itemFields.Add(TierField, typeof(string));
        }

        /// <summary>
        /// The cloud provider where data is stored
        /// </summary>
        /// <value></value>
        public string Platform
        {
            get { return StacPropertiesContainer.GetProperty<string>(PlatformField); }
            set { StacPropertiesContainer.SetProperty(PlatformField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// The region where the data is stored. Relevant to speed of access and inter region egress costs (as defined by PaaS provider)
        /// </summary>
        /// <value></value>
        public string Region
        {
            get { return StacPropertiesContainer.GetProperty<string>(RegionField); }
            set { StacPropertiesContainer.SetProperty(RegionField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// Is the data requester pays or is it data manager/cloud provider pays. Defaults to false
        /// </summary>
        /// <value></value>
        public bool? RequesterPays
        {
            get { return StacPropertiesContainer.GetProperty<bool?>(RequesterPaysField); }
            set { StacPropertiesContainer.SetProperty(RequesterPaysField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// The title for the tier type (as defined by PaaS provider)
        /// </summary>
        /// <value></value>
        public string Tier
        {
            get { return StacPropertiesContainer.GetProperty<string>(TierField); }
            set { StacPropertiesContainer.SetProperty(TierField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// Potential fields and their types
        /// </summary>
        public override IDictionary<string, Type> ItemFields => itemFields;


        public override IDictionary<string, ISummaryFunction> GetSummaryFunctions()
        {
            Dictionary<string, ISummaryFunction> summaryFunctions = new Dictionary<string, ISummaryFunction>();
            return summaryFunctions;
        }

    }

    /// <summary>
    /// Extension methods for accessing Storage extension
    /// </summary>
    public static class StorageStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a StorageStacExtension class from a STAC Asset
        /// </summary>
        public static StorageStacExtension StorageExtension(this StacAsset stacAsset)
        {
            return new StorageStacExtension(stacAsset);
        }

        /// <summary>
        /// Initilize a StorageStacExtension class from a STAC Item
        /// </summary>
        public static StorageStacExtension StorageExtension(this StacItem stacItem)
        {
            return new StorageStacExtension(stacItem);
        }

    }
}

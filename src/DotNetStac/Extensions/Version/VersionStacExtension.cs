using System;
using System.Collections.Generic;
using System.Linq;

namespace Stac.Extensions.Version
{
    /// <summary>
    /// Helper class to access the fields defined by the <seealso href="https://github.com/stac-extensions/version">Version extension</seealso>
    /// </summary>
    public class VersionStacExtension : StacPropertiesContainerExtension, IStacExtension
    {

        /// Extension identifier and schema url
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/version/v1.0.0/schema.json";

        internal const string VersionField = "version";
        internal const string DeprecatedField = "deprecated";

        internal const string PredecessorVersionRel = "predecessor-version";

        internal const string SuccessorVersionRel = "predecessor-version";

        internal const string LatestVersionRel = "predecessor-version";

        private readonly Dictionary<string, Type> itemFields;

        internal VersionStacExtension(IStacObject stacObject) : base(JsonSchemaUrl, stacObject)
        {
            itemFields = new Dictionary<string, Type>();
            itemFields.Add(VersionField, typeof(string));
            itemFields.Add(DeprecatedField, typeof(bool));
        }

        /// <summary>
        /// Version of the Collection or Item.
        /// </summary>
        /// <value></value>
        public string Version
        {
            get { return StacPropertiesContainer.GetProperty<string>(VersionField); }
            set { StacPropertiesContainer.SetProperty(VersionField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// Specifies that the Collection or Item is deprecated with the potential to be removed. 
        /// </summary>
        /// <value></value>
        public bool Deprecated
        {
            get { return StacPropertiesContainer.GetProperty<bool>(DeprecatedField); }
            set { StacPropertiesContainer.SetProperty(DeprecatedField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// Potential fields and their types
        /// </summary>
        public override IDictionary<string, Type> ItemFields => itemFields;

    }

    /// <summary>
    /// Extension methods for accessing EO extension
    /// </summary>
    public static class VersionStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a EoStacExtension class from a STAC item
        /// </summary>
        public static VersionStacExtension VersionExtension(this StacItem stacItem)
        {
            return new VersionStacExtension(stacItem);
        }

        /// <summary>
        /// Initilize a EoStacExtension class from a STAC collection
        /// </summary>
        public static VersionStacExtension VersionExtension(this StacCollection stacItem)
        {
            return new VersionStacExtension(stacItem);
        }

        /// <summary>
        /// Retrieve the predecessor version of the Stac Item if any
        /// </summary>
        /// <param name="stacItem">current Stac Item</param>
        /// <param name="stacLinkResolver">Function that read a Stac Link to get the StacItem</param>
        /// <returns>null if no precedessor version</returns>
        public static StacItem PredecessorVersion(this StacItem stacItem, Func<StacLink, StacItem> stacLinkResolver)
        {
            return GetVersion<StacItem>(stacItem, VersionStacExtension.PredecessorVersionRel, stacLinkResolver);
        }

        /// <summary>
        /// Retrieve the predecessor version of the Stac Collection if any
        /// </summary>
        /// <param name="stacCollection">current Stac Collection</param>
        /// <param name="stacLinkResolver">Function that read a Stac Link to get the StacItem</param>
        /// <returns>null if no precedessor version</returns>
        public static StacCollection PredecessorVersion(this StacCollection stacCollection, Func<StacLink, StacCollection> stacLinkResolver)
        {
            return GetVersion<StacCollection>(stacCollection, VersionStacExtension.PredecessorVersionRel, stacLinkResolver);
        }

        /// <summary>
        /// Retrieve the successor version of the Stac Item if any
        /// </summary>
        /// <param name="stacItem">current Stac Item</param>
        /// <param name="stacLinkResolver">Function that read a Stac Link to get the StacItem</param>
        /// <returns>null if no successor version</returns>
        public static StacItem SuccessorVersion(this StacItem stacItem, Func<StacLink, StacItem> stacLinkResolver)
        {
            return GetVersion<StacItem>(stacItem, VersionStacExtension.SuccessorVersionRel, stacLinkResolver);
        }

        /// <summary>
        /// Retrieve the successor version of the Stac Collection if any
        /// </summary>
        /// <param name="stacCollection">current Stac Collection</param>
        /// <param name="stacLinkResolver">Function that read a Stac Link to get the StacItem</param>
        /// <returns>null if no successor version</returns>
        public static StacCollection SuccessorVersion(this StacCollection stacCollection, Func<StacLink, StacCollection> stacLinkResolver)
        {
            return GetVersion<StacCollection>(stacCollection, VersionStacExtension.SuccessorVersionRel, stacLinkResolver);
        }

        /// <summary>
        /// Retrieve the latest version of the Stac Item if any
        /// </summary>
        /// <param name="stacItem">current Stac Item</param>
        /// <param name="stacLinkResolver">Function that read a Stac Link to get the StacItem</param>
        /// <returns>null if no latest version</returns>
        public static StacItem LatestVersion(this StacItem stacItem, Func<StacLink, StacItem> stacLinkResolver)
        {
            return GetVersion<StacItem>(stacItem, VersionStacExtension.LatestVersionRel, stacLinkResolver);
        }

        /// <summary>
        /// Retrieve the latest version of the Stac Collection if any
        /// </summary>
        /// <param name="stacCollection">current Stac Collection</param>
        /// <param name="stacLinkResolver">Function that read a Stac Link to get the StacItem</param>
        /// <returns>null if no latest version</returns>
        public static StacCollection LatestVersion(this StacCollection stacCollection, Func<StacLink, StacCollection> stacLinkResolver)
        {
            return GetVersion<StacCollection>(stacCollection, VersionStacExtension.LatestVersionRel, stacLinkResolver);
        }

        internal static T GetVersion<T>(this T stacObject, string relType, Func<StacLink, T> stacLinkResolver) where T : IStacObject
        {
            var predecessorVersionLink = stacObject.Links.FirstOrDefault(l => l.RelationshipType == relType);
            if (predecessorVersionLink == null) return default(T);
            return stacLinkResolver(predecessorVersionLink);
        }
    }
}

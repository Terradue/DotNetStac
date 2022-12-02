using System;
using System.Collections.Generic;
using System.Linq;

namespace Stac.Extensions.VirtualAssets
{
    /// <summary>
    /// Helper class to access the fields deined by the <seealso href="https://github.com/stac-extensions/eo">EO extension</seealso>
    /// </summary>
    public class VirtualAssetsStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        /// Extension identifier and schema url
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/virtual-assets/v1.0.0/schema.json";

        private IDictionary<string, Type> itemFields;
        private const string VirtualAssetsField = "virtual:assets";

        internal VirtualAssetsStacExtension(IStacObject stacObject) : base(JsonSchemaUrl, stacObject)
        {
            itemFields = new Dictionary<string, Type>();
            itemFields.Add(VirtualAssetsField, typeof(VirtualAsset));
        }

        /// <summary>
        /// Virtual Assets
        /// </summary>
        public IDictionary<string, VirtualAsset> Assets
        {
            get { return StacPropertiesContainer.GetProperty<IDictionary<string, VirtualAsset>>(VirtualAssetsField); }
            set { StacPropertiesContainer.SetProperty(VirtualAssetsField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// Potential fields and their types
        /// </summary>
        public override IDictionary<string, Type> ItemFields => itemFields;
    }

    /// <summary>
    /// Extension methods for accessing EO extension
    /// </summary>
    public static class VirtualAssetsStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a VirtualAssetsStacExtension class from a STAC item
        /// </summary>
        public static VirtualAssetsStacExtension EoExtension(this StacItem stacItem)
        {
            return new VirtualAssetsStacExtension(stacItem);
        }

        /// <summary>
        /// Initilize a VirtualAssetsStacExtension class from a STAC collection
        /// </summary>
        public static VirtualAssetsStacExtension EoExtension(this StacCollection stacCollection)
        {
            return new VirtualAssetsStacExtension(stacCollection);
        }
    }
}

using System;
using System.Collections.Generic;

namespace Stac.Extensions.Disaster
{
    /// <summary>
    /// Disasters Charter Extension to the SpatioTemporal Asset Catalog (STAC) specification.
    /// This extension provides with:
    /// * Additional fields for common disaster properties such as type (e.g. cyclone, earthquake, flooding...).
    /// * Additional fields for specific metadata for the charter such as the call or activation identifier.
    /// * Best practises to describe several objects used in the Disasters Charter (Activation, Disaster Area, Acquisition...).
    /// </summary>
    public class DisastersCharterStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        /// Current schema url
        public const string JsonSchemaUrl = "https://terradue.github.io/stac-extensions-disaster/v1.0.0/schema.json";

        private static IDictionary<string, Type> itemFields;

        // Activation id field name
        public const string ActivationIdField = "disaster:activation_id";
        // Call ids field name
        public const string CallIdsField = "disaster:call_ids";
        // Country field name
        public const string CountryField = "disaster:country";
        // Regions field name
        public const string RegionField = "disaster:regions";
        // Types field name
        public const string TypeField = "disaster:types";
        // Class field name
        public const string ClassField = "disaster:class";
        // Activation status field name
        public const string ActivationStatusField = "disaster:activation_status";
        // Resolution class field name
        public const string ResolutionClassField = "disaster:resolution_class";

        /// <summary>
        /// Initializes the <see cref="DisastersCharterStacExtension"/> class.
        /// </summary>
        /// <param name="stacpropertiesContainer"></param>
        public DisastersCharterStacExtension(IStacPropertiesContainer stacpropertiesContainer) : base(JsonSchemaUrl, stacpropertiesContainer)
        {
            if (itemFields == null)
            {
                itemFields = new Dictionary<string, Type>();
                itemFields.Add(ActivationIdField, typeof(int));
                itemFields.Add(CallIdsField, typeof(int[]));
                itemFields.Add(CountryField, typeof(string));
                itemFields.Add(RegionField, typeof(IEnumerable<string>));
                itemFields.Add(TypeField, typeof(IEnumerable<DisastersType>));
                itemFields.Add(ClassField, typeof(DisastersItemClass?));
                itemFields.Add(ActivationStatusField, typeof(DisastersActivationStatus?));
                itemFields.Add(ResolutionClassField, typeof(DisastersResolutionClass?));
            }
        }

        /// <summary>
        /// Potential fields and their types
        /// </summary>
        public override IDictionary<string, Type> ItemFields => itemFields;

        /// <summary>
        /// Identifier of the related Activation
        /// </summary>
        public int ActivationId
        {
            get { return StacPropertiesContainer.GetProperty<int>(ActivationIdField); }
            set { StacPropertiesContainer.SetProperty(ActivationIdField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// Identifiers of the related Call(s)
        /// </summary>
        public int[] CallIds
        {
            get { return StacPropertiesContainer.GetProperty<int[]>(CallIdsField); }
            set { StacPropertiesContainer.SetProperty(CallIdsField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// Related Country identifier based on the ISO-3166 standard. In particular, the Alpha-3 representation. (e.g. BEL)
        /// </summary>
        public string Country
        {
            get { return StacPropertiesContainer.GetProperty<string>(CountryField); }
            set
            {
                // check if the country is valid by regex
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[A-Z]{3}$"))
                    throw new ArgumentException("Country must be a valid ISO-3166 Alpha-3 code");
                StacPropertiesContainer.SetProperty(CountryField, value); DeclareStacExtension();
            }
        }

        /// <summary>
        /// Free text list identifying regions
        /// </summary>
        public IEnumerable<string> Regions
        {
            get { return StacPropertiesContainer.GetProperty<IEnumerable<string>>(RegionField); }
            set { StacPropertiesContainer.SetProperty(RegionField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// Disaster Types (one of the category)
        /// </summary>
        public IEnumerable<DisastersType> Types
        {
            get { try { return (IEnumerable<DisastersType>)StacPropertiesContainer.GetProperty<IEnumerable<DisastersType>>(TypeField); } catch { return null; } }
            set { StacPropertiesContainer.SetProperty(TypeField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// Class of the object(s) described in the item or collection.
        /// </summary>
        public DisastersItemClass? Class
        {
            get { return StacPropertiesContainer.GetProperty<DisastersItemClass?>(ClassField); }
            set { StacPropertiesContainer.SetProperty(ClassField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// Activation status.
        /// </summary>
        public DisastersActivationStatus? ActivationStatus
        {
            get { return StacPropertiesContainer.GetProperty<DisastersActivationStatus?>(ActivationStatusField); }
            set { StacPropertiesContainer.SetProperty(ActivationStatusField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// Resolution class for an Acquisition Item
        /// </summary>
        public DisastersResolutionClass? ResolutionClass
        {
            get { return StacPropertiesContainer.GetProperty<DisastersResolutionClass?>(ResolutionClassField); }
            set { StacPropertiesContainer.SetProperty(ResolutionClassField, value); DeclareStacExtension(); }
        }
    }

    /// <summary>
    /// Extension methods for accessing EO extension
    /// </summary>
    public static class DisasterStacExtensionExtensions
    {

        /// <summary>
        /// Initilize a EoStacExtension class from a STAC item
        /// </summary>
        public static DisastersCharterStacExtension DisasterExtension(this IStacObject stacObject)
        {
            return new DisastersCharterStacExtension(stacObject);
        }

        /// <summary>
        /// Initilize a DisastersCharterStacExtension
        /// </summary>
        /// <param name="disasterStacExtension"></param>
        /// <param name="disastersItemClass"></param>
        /// <param name="activationId"></param>
        /// <param name="callIds"></param>
        public static void Init(this DisastersCharterStacExtension disasterStacExtension, DisastersItemClass disastersItemClass, int activationId, int[] callIds)
        {
            disasterStacExtension.Class = disastersItemClass;
            disasterStacExtension.ActivationId = activationId;
            disasterStacExtension.CallIds = callIds;
        }
    }
}

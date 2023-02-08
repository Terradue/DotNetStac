// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DisastersCharterStacExtension.cs

using System;
using System.Collections.Generic;

namespace Stac.Extensions.Disaster
{
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

    /// <summary>
    /// Disasters Charter Extension to the SpatioTemporal Asset Catalog (STAC) specification.
    /// This extension provides with:
    /// * Additional fields for common disaster properties such as type (e.g. cyclone, earthquake, flooding...).
    /// * Additional fields for specific metadata for the charter such as the call or activation identifier.
    /// * Best practises to describe several objects used in the Disasters Charter (Activation, Disaster Area, Acquisition...).
    /// </summary>
    public class DisastersCharterStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        // Current schema url
        public const string JsonSchemaUrl = "https://terradue.github.io/stac-extensions-disaster/v1.0.0/schema.json";

        // Activation id field name
        public const string ActivationIdField = "disaster:activation_id";

        private static IDictionary<string, Type> itemFields;

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
        /// Initializes a new instance of the <see cref="DisastersCharterStacExtension"/> class.
        /// Initializes the <see cref="DisastersCharterStacExtension"/> class.
        /// </summary>
        /// <param name="stacpropertiesContainer"></param>
        public DisastersCharterStacExtension(IStacPropertiesContainer stacpropertiesContainer)
            : base(JsonSchemaUrl, stacpropertiesContainer)
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
        /// Gets potential fields and their types
        /// </summary>
        public override IDictionary<string, Type> ItemFields => itemFields;

        /// <summary>
        /// Gets or sets identifier of the related Activation
        /// </summary>
        public int ActivationId
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<int>(ActivationIdField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ActivationIdField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets identifiers of the related Call(s)
        /// </summary>
        public int[] CallIds
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<int[]>(CallIdsField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(CallIdsField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets related Country identifier based on the ISO-3166 standard. In particular, the Alpha-3 representation. (e.g. BEL)
        /// </summary>
        public string Country
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<string>(CountryField);
            }

            set
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[A-Z]{3}$"))
                {
                    throw new ArgumentException("Country must be a valid ISO-3166 Alpha-3 code");
                }
                this.StacPropertiesContainer.SetProperty(CountryField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets free text list identifying regions
        /// </summary>
        public IEnumerable<string> Regions
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<IEnumerable<string>>(RegionField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(RegionField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets disaster Types (one of the category)
        /// </summary>
        public IEnumerable<DisastersType> Types
        {
            get { try
{
    return (IEnumerable<DisastersType>)this.StacPropertiesContainer.GetProperty<IEnumerable<DisastersType>>(TypeField);
}
catch
{
    return null;
}
}
            set { this.StacPropertiesContainer.SetProperty(TypeField, value);
                this.DeclareStacExtension(); }
        }

        /// <summary>
        /// Gets or sets class of the object(s) described in the item or collection.
        /// </summary>
        public DisastersItemClass? Class
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<DisastersItemClass?>(ClassField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ClassField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets activation status.
        /// </summary>
        public DisastersActivationStatus? ActivationStatus
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<DisastersActivationStatus?>(ActivationStatusField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ActivationStatusField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets resolution class for an Acquisition Item
        /// </summary>
        public DisastersResolutionClass? ResolutionClass
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<DisastersResolutionClass?>(ResolutionClassField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ResolutionClassField, value);
                this.DeclareStacExtension();
            }
        }
    }
}

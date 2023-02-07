// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: VersionStacExtension.cs

using System;
using System.Collections.Generic;

namespace Stac.Extensions.Version
{
    /// <summary>
    /// Helper class to access the fields defined by the <seealso href="https://github.com/stac-extensions/version">Version extension</seealso>
    /// </summary>
    public class VersionStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
#pragma warning disable CS1591 // Elements should be documented
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/version/v1.0.0/schema.json";

        public const string VersionField = "version";
        public const string DeprecatedField = "deprecated";

        public const string PredecessorVersionRel = "predecessor-version";

        public const string SuccessorVersionRel = "predecessor-version";

        public const string LatestVersionRel = "predecessor-version";

#pragma warning disable CS1591 // Elements should be documented

        private readonly Dictionary<string, Type> _itemFields;

        internal VersionStacExtension(IStacObject stacObject)
            : base(JsonSchemaUrl, stacObject)
        {
            this._itemFields = new Dictionary<string, Type>();
            this._itemFields.Add(VersionField, typeof(string));
            this._itemFields.Add(DeprecatedField, typeof(bool));
        }

        /// <summary>
        /// Gets or sets version of the Collection or Item.
        /// </summary>
        /// <value>
        /// Version of the Collection or Item.
        /// </value>
        public string Version
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<string>(VersionField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(VersionField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether specifies that the Collection or Item is deprecated with the potential to be removed.
        /// </summary>
        /// <value>
        /// A value indicating whether specifies that the Collection or Item is deprecated with the potential to be removed.
        /// </value>
        public bool Deprecated
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<bool>(DeprecatedField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(DeprecatedField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets potential fields and their types
        /// </summary>
        /// <value>
        /// Potential fields and their types
        /// </value>
        public override IDictionary<string, Type> ItemFields => this._itemFields;

        /// <inheritdoc/>
        public override IDictionary<string, ISummaryFunction> GetSummaryFunctions()
        {
            Dictionary<string, ISummaryFunction> summaryFunctions = new Dictionary<string, ISummaryFunction>();
            return summaryFunctions;
        }
    }
}

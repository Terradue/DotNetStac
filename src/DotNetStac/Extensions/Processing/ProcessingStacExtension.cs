using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Stac.Model;

namespace Stac.Extensions.Processing
{
    /// <summary>
    /// Helper class to access the fields defined by the <seealso href="https://github.com/stac-extensions/processing">Processing extension</seealso>
    /// </summary>
    public class ProcessingStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        /// Extension identifier and schema url
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/processing/v1.0.0/schema.json";
        private const string LineageField = "processing:lineage";
        private const string LevelField = "processing:level";
        private const string FacilityField = "processing:facility";
        private const string SoftwareField = "processing:software";
        private readonly Dictionary<string, Type> itemFields;

        internal ProcessingStacExtension(StacItem stacItem) : base(JsonSchemaUrl, stacItem)
        {
            itemFields = new Dictionary<string, Type>();
            itemFields.Add(LineageField, typeof(string));
            itemFields.Add(LevelField, typeof(string));
            itemFields.Add(FacilityField, typeof(string));
            itemFields.Add(SoftwareField, typeof(IDictionary<string, string>));
        }

        /// <summary>
        /// Lineage Information provided as free text information about the how observations were processed or models that were used to create the resource being described NASA ISO.
        /// </summary>
        /// <value></value>
        public string Lineage
        {
            get { return StacPropertiesContainer.GetProperty<string>(LineageField); }
            set { StacPropertiesContainer.SetProperty(LineageField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// The name commonly used to refer to the processing level to make it easier to search for product level across collections or items.
        /// </summary>
        /// <value></value>
        public string Level
        {
            get { return StacPropertiesContainer.GetProperty<string>(LevelField); }
            set { StacPropertiesContainer.SetProperty(LevelField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// The name of the facility that produced the data. 
        /// </summary>
        /// <value></value>
        public string Facility
        {
            get { return StacPropertiesContainer.GetProperty<string>(FacilityField); }
            set { StacPropertiesContainer.SetProperty(FacilityField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// A dictionary with name/version for key/value describing one or more softwares that produced the data.
        /// </summary>
        /// <value></value>
        public IDictionary<string, string> Software
        {
            get
            {
                Dictionary<string, string> existingSoftware = StacPropertiesContainer.GetProperty<Dictionary<string, string>>(SoftwareField);
                ObservableDictionary<string, string> software = null;
                if (existingSoftware == null)
                    software = new ObservableDictionary<string, string>();
                else
                    software = new ObservableDictionary<string, string>(existingSoftware);
                software.CollectionChanged += UpdateSoftwareField;
                return software;
            }
        }

        /// <summary>
        /// Potential fields and their types
        /// </summary>
        public override IDictionary<string, Type> ItemFields => itemFields;


        private void UpdateSoftwareField(object sender, NotifyCollectionChangedEventArgs e)
        {
            StacPropertiesContainer.SetProperty(SoftwareField, new Dictionary<string, string>(sender as IDictionary<string, string>));
            DeclareStacExtension();
        }

    }

    /// <summary>
    /// Extension methods for accessing Processing extension
    /// </summary>
    public static class ProcessingStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a EoStacExtension class from a STAC item
        /// </summary>
        public static ProcessingStacExtension ProcessingExtension(this StacItem stacItem)
        {
            return new ProcessingStacExtension(stacItem);
        }

        /// <summary>
        /// Initialize the major fields of processing extensions
        /// </summary>
        public static void Init(this ProcessingStacExtension processingStacExtension,
                                string lineage,
                                string level,
                                string facility = null)
        {
            processingStacExtension.Lineage = lineage;
            processingStacExtension.Level = level;
            processingStacExtension.Facility = facility;
        }
    }
}

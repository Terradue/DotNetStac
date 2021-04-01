using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Stac.Model;

namespace Stac.Extensions.Processing
{
    public class ProcessingStacExtension : StacPropertiesContainerExtension, IStacExtension
    {

        public const string JsonSchemaUrl = "https://stac-extensions.github.io/processing/v1.0.0/schema.json";
        public const string LineageField = "processing:lineage";
        public const string LevelField = "processing:level";
        public const string FacilityField = "processing:facility";
        public const string SoftwareField = "processing:software";
        private readonly Dictionary<string, Type> itemFields;

        public ProcessingStacExtension(StacItem stacItem) : base(JsonSchemaUrl, stacItem)
        {
            itemFields = new Dictionary<string, Type>();
            itemFields.Add(LineageField, typeof(string));
            itemFields.Add(LevelField, typeof(string));
            itemFields.Add(FacilityField, typeof(string));
            itemFields.Add(SoftwareField, typeof(IDictionary<string, string>));
        }

        public string Lineage
        {
            get { return StacPropertiesContainer.GetProperty<string>(LineageField); }
            set { StacPropertiesContainer.SetProperty(LineageField, value); DeclareStacExtension(); }
        }

        public string Level
        {
            get { return StacPropertiesContainer.GetProperty<string>(LevelField); }
            set { StacPropertiesContainer.SetProperty(LevelField, value); DeclareStacExtension(); }
        }

        public string Facility
        {
            get { return StacPropertiesContainer.GetProperty<string>(FacilityField); }
            set { StacPropertiesContainer.SetProperty(FacilityField, value); DeclareStacExtension(); }
        }

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

        public override IDictionary<string, Type> ItemFields => itemFields;

        private void UpdateSoftwareField(object sender, NotifyCollectionChangedEventArgs e)
        {
            StacPropertiesContainer.SetProperty(SoftwareField, new Dictionary<string, string>(sender as IDictionary<string, string>));
            DeclareStacExtension();
        }

    }

    public static class ProcessingStacExtensionExtensions
    {
        public static ProcessingStacExtension ProcessingExtension(this StacItem stacItem)
        {
            return new ProcessingStacExtension(stacItem);
        }

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

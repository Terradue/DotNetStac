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
            set { StacPropertiesContainer.SetProperty(LineageField, value); }
        }

        public string Level
        {
            get { return StacPropertiesContainer.GetProperty<string>(LevelField); }
            set { StacPropertiesContainer.SetProperty(LevelField, value); }
        }

        public string Facility
        {
            get { return StacPropertiesContainer.GetProperty<string>(FacilityField); }
            set { StacPropertiesContainer.SetProperty(FacilityField, value); }
        }

        public IDictionary<string, string> Software
        {
            get
            {
                IDictionary<string, string> existingSoftware = StacPropertiesContainer.GetProperty<IDictionary<string, string>>(FacilityField);
                ObservableDictionary<string, string> software = null;
                if (software == null)
                    software = new ObservableDictionary<string, string>();
                else
                    software = new ObservableDictionary<string, string>(software);
                software.CollectionChanged += UpdateSoftwareField;
                return software;
            }
        }

        public override IDictionary<string, Type> ItemFields => itemFields;

        private void UpdateSoftwareField(object sender, NotifyCollectionChangedEventArgs e)
        {
            StacPropertiesContainer.SetProperty(SoftwareField, sender as IDictionary<string, string>);
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

        }
    }
}

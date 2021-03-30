using System.Collections.Generic;
using System.Collections.Specialized;
using Stac.Model;

namespace Stac.Extensions.Processing
{
    public class ProcessingStacExtension : StacPropertiesContainerExtension, IStacExtension
    {

        public const string JsonSchemaUrl = "https://stac-extensions.github.io/processing/v1.0.0/schema.json";
        public const string Prefix = "processing";
        public const string LineageField = "lineage";
        public const string LevelField = "level";
        public const string FacilityField = "facility";
        public const string SoftwareField = "software";

        private readonly ObservableDictionary<string, string> software = new ObservableDictionary<string, string>();

        public ProcessingStacExtension(IStacObject stacObject) : base(Prefix, stacObject)
        {
            var existingSoftware = GetField<Dictionary<string, string>>(SoftwareField);
            if (existingSoftware != null)
                software = new ObservableDictionary<string, string>(existingSoftware);
            software.CollectionChanged += UpdateSoftwareField;
        }

        public ProcessingStacExtension(IStacObject stacObject, string lineage, string level, string facility = null) : this(stacObject)
        {

        }

        public string Lineage
        {
            get { return base.GetField<string>(LineageField); }
            set { base.SetField(LineageField, value); }
        }

        public string Level
        {
            get { return base.GetField<string>(LevelField); }
            set { base.SetField(LevelField, value); }
        }

        public string Facility
        {
            get { return base.GetField<string>(FacilityField); }
            set { base.SetField(FacilityField, value); }
        }

        public IDictionary<string, string> Software => software;

        private void UpdateSoftwareField(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.SetField(SoftwareField, software);
        }

    }
}

// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: PropertyObservableCollection.cs

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Stac.Common
{
    public class PropertyObservableCollection<T> : ObservableCollection<T>
    {
        public PropertyObservableCollection(IStacPropertiesContainer propertiesContainer, string key) : base()
        {
            PropertiesContainer = propertiesContainer;
            Key = key;
            this.CollectionChanged += ObservableCollectionInPropertiesChanged;
        }

        public IStacPropertiesContainer PropertiesContainer { get; }
        public string Key { get; }

        private void ObservableCollectionInPropertiesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertiesContainer.RemoveProperty(Key);
            if (this.Count == 0) return;
            PropertiesContainer.SetProperty(Key, this.ToList());
        }
    }
}

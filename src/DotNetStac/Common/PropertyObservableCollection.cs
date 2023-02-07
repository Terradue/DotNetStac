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
            this.PropertiesContainer = propertiesContainer;
            this.Key = key;
            this.CollectionChanged += this.ObservableCollectionInPropertiesChanged;
        }

        public IStacPropertiesContainer PropertiesContainer { get; }
        public string Key { get; }

        private void ObservableCollectionInPropertiesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.PropertiesContainer.RemoveProperty(this.Key);
            if (this.Count == 0) return;
            this.PropertiesContainer.SetProperty(this.Key, this.ToList());
        }
    }
}

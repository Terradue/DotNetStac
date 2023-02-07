// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: PropertyObservableCollection.cs

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Stac.Common
{
    /// <summary>
    /// A collection that is observable and that is also stored in a <see cref="IStacPropertiesContainer"/> as a property.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class PropertyObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="propertiesContainer">The properties container.</param>
        /// <param name="key">The property key.</param>
        public PropertyObservableCollection(IStacPropertiesContainer propertiesContainer, string key) : base()
        {
            this.PropertiesContainer = propertiesContainer;
            this.Key = key;
            this.CollectionChanged += this.ObservableCollectionInPropertiesChanged;
        }

        /// <summary>
        /// Gets the properties container.
        /// </summary>
        /// <value>
        /// <placeholder>The properties container.</placeholder>
        /// </value>
        public IStacPropertiesContainer PropertiesContainer { get; }

        /// <summary>
        /// Gets the property key.
        /// </summary>
        /// <value>
        /// <placeholder>The property key.</placeholder>
        /// </value>
        public string Key { get; }

        private void ObservableCollectionInPropertiesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.PropertiesContainer.RemoveProperty(this.Key);
            if (this.Count == 0)
            {
                return;
            }

            this.PropertiesContainer.SetProperty(this.Key, this.ToList());
        }
    }
}

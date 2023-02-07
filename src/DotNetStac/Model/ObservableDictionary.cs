// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ObservableDictionary.cs

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Stac.Model
{
    /// <summary>
    /// Provides a thread-safe dictionary for use with data binding.
    /// </summary>
    /// <typeparam name="TKey">Specifies the type of the keys in this collection.</typeparam>
    /// <typeparam name="TValue">Specifies the type of the values in this collection.</typeparam>
    [DebuggerDisplay("Count={Count}")]
    public class ObservableDictionary<TKey, TValue> :
        ICollection<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>,
        INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly SynchronizationContext _context;
        private readonly ConcurrentDictionary<TKey, TValue> _dictionary;

        /// <summary>
        /// Initializes an instance of the ObservableConcurrentDictionary class.
        /// </summary>
        public ObservableDictionary()
        {
            this._context = AsyncOperationManager.SynchronizationContext;
            this._dictionary = new ConcurrentDictionary<TKey, TValue>();
        }

        public ObservableDictionary(IDictionary<TKey, TValue> init)
            : this()
        {
            this._context = AsyncOperationManager.SynchronizationContext;
            this._dictionary = new ConcurrentDictionary<TKey, TValue>(init);
        }

        /// <summary>Event raised when the collection changes.</summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>Event raised when a property on the collection changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        int ICollection<KeyValuePair<TKey, TValue>>.Count
        {
            get => this._dictionary.Count;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get => ((ICollection<KeyValuePair<TKey, TValue>>)this._dictionary).IsReadOnly;
        }

        /// <inheritdoc/>
        public ICollection<TKey> Keys
        {
            get { return this._dictionary.Keys; }
        }

        /// <summary>
        /// Notifies observers of CollectionChanged or PropertyChanged of an update to the dictionary.
        /// </summary>
        private void NotifyObserversOfChange()
        {
            var collectionHandler = this.CollectionChanged;
            var propertyHandler = this.PropertyChanged;
            if (collectionHandler != null || propertyHandler != null)
            {
                this._context.Send(s =>
                {
                    collectionHandler?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    propertyHandler?.Invoke(this, new PropertyChangedEventArgs("Count"));
                    propertyHandler?.Invoke(this, new PropertyChangedEventArgs("Keys"));
                    propertyHandler?.Invoke(this, new PropertyChangedEventArgs("Values"));
                }, null);
            }
        }

        /// <summary>
        /// Notifies observers of CollectionChanged or PropertyChanged of an update to the dictionary.
        /// </summary>
        /// <param name="actionType">Add or Update action</param>
        /// <param name="changedItem">The item involved with the change</param>
        private void NotifyObserversOfChange(NotifyCollectionChangedAction actionType, object changedItem)
        {
            var collectionHandler = this.CollectionChanged;
            var propertyHandler = this.PropertyChanged;
            if (collectionHandler != null || propertyHandler != null)
            {
                this._context.Send(s =>
                {
                    collectionHandler?.Invoke(this, new NotifyCollectionChangedEventArgs(actionType, changedItem));
                    propertyHandler?.Invoke(this, new PropertyChangedEventArgs("Count"));
                    propertyHandler?.Invoke(this, new PropertyChangedEventArgs("Keys"));
                    propertyHandler?.Invoke(this, new PropertyChangedEventArgs("Values"));
                }, null);
            }
        }

        /// <summary>
        /// Notifies observers of CollectionChanged or PropertyChanged of an update to the dictionary.
        /// </summary>
        /// <param name="actionType">Remove action or optionally an Add action</param>
        /// <param name="item">The item in question</param>
        /// <param name="index">The position of the item in the collection</param>
        private void NotifyObserversOfChange(NotifyCollectionChangedAction actionType, object item, int index)
        {
            var collectionHandler = this.CollectionChanged;
            var propertyHandler = this.PropertyChanged;
            if (collectionHandler != null || propertyHandler != null)
            {
                this._context.Send(s =>
                {
                    collectionHandler?.Invoke(this, new NotifyCollectionChangedEventArgs(actionType, item, index));
                    propertyHandler?.Invoke(this, new PropertyChangedEventArgs("Count"));
                    propertyHandler?.Invoke(this, new PropertyChangedEventArgs("Keys"));
                    propertyHandler?.Invoke(this, new PropertyChangedEventArgs("Values"));
                }, null);
            }
        }

        /// <summary>Attempts to add an item to the dictionary, notifying observers of any changes.</summary>
        /// <param name="item">The item to be added.</param>
        /// <returns>Whether the add was successful.</returns>
        private bool TryAddWithNotification(KeyValuePair<TKey, TValue> item)
            => this.TryAddWithNotification(item.Key, item.Value);

        /// <summary>Attempts to add an item to the dictionary, notifying observers of any changes.</summary>
        /// <param name="key">The key of the item to be added.</param>
        /// <param name="value">The value of the item to be added.</param>
        /// <returns>Whether the add was successful.</returns>
        private bool TryAddWithNotification(TKey key, TValue value)
        {
            bool result = this._dictionary.TryAdd(key, value);
            int index = this.IndexOf(key);
            if (result)
            {
                this.NotifyObserversOfChange(NotifyCollectionChangedAction.Add, value, index);
            }

            return result;
        }

        /// <summary>Attempts to remove an item from the dictionary, notifying observers of any changes.</summary>
        /// <param name="key">The key of the item to be removed.</param>
        /// <param name="value">The value of the item removed.</param>
        /// <returns>Whether the removal was successful.</returns>
        private bool TryRemoveWithNotification(TKey key, out TValue value)
        {
            int index = this.IndexOf(key);
            bool result = this._dictionary.TryRemove(key, out value);
            if (result)
            {
                this.NotifyObserversOfChange(NotifyCollectionChangedAction.Remove, value, index);
            }

            return result;
        }

        /// <summary>Attempts to add or update an item in the dictionary, notifying observers of any changes.</summary>
        /// <param name="key">The key of the item to be updated.</param>
        /// <param name="value">The new value to set for the item.</param>
        /// <returns>Whether the update was successful.</returns>
        private void UpdateWithNotification(TKey key, TValue value)
        {
            this._dictionary[key] = value;
            this.NotifyObserversOfChange(NotifyCollectionChangedAction.Replace, value);
        }

        /// <summary>
        /// WPF requires that the reported index for Add/Remove events are correct/reliable. With a dictionary there
        /// is no choice but to brute-force search through the key list. Ugly.
        /// </summary>
        private int IndexOf(TKey key)
        {
            var keys = this._dictionary.Keys;
            int index = -1;
            foreach (TKey k in keys)
            {
                index++;
                if (k.Equals(key))
                {
                    return index;
                }
            }

            return -1;
        }

        // ICollection<KeyValuePair<TKey,TValue>> Members

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
            => this.TryAddWithNotification(item);

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            this._dictionary.Clear();
            this.NotifyObserversOfChange();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
            => ((ICollection<KeyValuePair<TKey, TValue>>)this._dictionary).Contains(item);

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            => ((ICollection<KeyValuePair<TKey, TValue>>)this._dictionary).CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        public ICollection<TValue> Values
        {
            get => this._dictionary.Values;
        }

        /// <inheritdoc/>
        public TValue this[TKey key]
        {
            get => this._dictionary[key];
            set => this.UpdateWithNotification(key, value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
            => this.TryRemoveWithNotification(item.Key, out TValue temp);

        // IEnumerable<KeyValuePair<TKey,TValue>> Members

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
            => this._dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this._dictionary.GetEnumerator();

        // IDictionary<TKey,TValue> Members

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
            => this.TryAddWithNotification(key, value);

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
            => this._dictionary.ContainsKey(key);

        /// <inheritdoc/>
        public bool Remove(TKey key)
            => this.TryRemoveWithNotification(key, out TValue temp);

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out TValue value)
            => this._dictionary.TryGetValue(key, out value);
    }
}

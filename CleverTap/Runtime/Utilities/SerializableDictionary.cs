using System;
using System.Collections.Generic;
using System.Linq;

namespace CleverTapSDK.Utilities
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        /// <summary>
        /// Serializable representation of a key-value pair.
        /// </summary>
        [Serializable]
        public class KeyValuePair
        {
            /// <summary>
            /// The key of the pair.
            /// </summary>
            public TKey Key;

            /// <summary>
            /// The value of the pair.
            /// </summary>
            public TValue Value;
        }

        /// <summary>
        /// List of key-value pairs representing the dictionary.
        /// </summary>
        public List<KeyValuePair> Items = new List<KeyValuePair>();

        /// <summary>
        /// Creates a SerializableDictionary from a standard Dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary to convert.</param>
        /// <returns>A SerializableDictionary containing the same data.</returns>
        public static SerializableDictionary<TKey, TValue> FromDictionary(Dictionary<TKey, TValue> dictionary)
        {
            var serializable = new SerializableDictionary<TKey, TValue>();
            foreach (var kvp in dictionary)
            {
                serializable.Items.Add(new KeyValuePair { Key = kvp.Key, Value = kvp.Value });
            }
            return serializable;
        }

        /// <summary>
        /// Converts the SerializableDictionary to a standard Dictionary.
        /// </summary>
        /// <returns>A Dictionary containing the same data.</returns>
        public Dictionary<TKey, TValue> ToDictionary()
        {
            return Items.ToDictionary(item => item.Key, item => item.Value);
        }
    }
}
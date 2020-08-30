using System;
using System.Collections;
using System.Collections.Generic;

namespace zijian666.SuperConvert.Core
{
    public class PropertiesCollection : IReadOnlyList<PropertyHandler>, IReadOnlyDictionary<string, PropertyHandler>
    {
        public static PropertiesCollection Empty { get; } = new PropertiesCollection(new List<PropertyHandler>(0));

        private readonly List<PropertyHandler> _properties;

        public PropertiesCollection(List<PropertyHandler> properties)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public int GetKeyIndex(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return -1;
            }

            key = key.Trim();
            for (var i = 0; i < _properties.Count; i++)
            {
                if (_properties[i].Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }
        public bool ContainsKey(string key) => GetKeyIndex(key) >= 0;

        public bool TryGetValue(string key, out PropertyHandler value)
        {
            var index = GetKeyIndex(key);
            if (index == -1)
            {
                value = null;
                return false;
            }

            value = _properties[index];
            return true;
        }

        public PropertyHandler this[string key] => this[GetKeyIndex(key)];

        public IEnumerable<string> Keys
        {
            get
            {
                foreach (var property in _properties)
                {
                    yield return property.Name;
                }
            }
        }

        public IEnumerable<PropertyHandler> Values => _properties;

        IEnumerator<KeyValuePair<string, PropertyHandler>> IEnumerable<KeyValuePair<string, PropertyHandler>>.
            GetEnumerator()
        {
            foreach (var property in _properties)
            {
                yield return new KeyValuePair<string, PropertyHandler>(property.Name, property);
            }
        }

        public PropertyHandler this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    return null;
                }
                return _properties[index];
            }
        }

        public int Count => _properties.Count;

        public IEnumerator<PropertyHandler> GetEnumerator() => _properties.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _properties.GetEnumerator();
    }
}

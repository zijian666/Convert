using System.Collections;

namespace zijian666.SuperConvert.Core
{
    public class StringSeparator
    {
        private readonly ArrayList _separators;

        private StringSeparator(string separator)
        {
            _separators = ArrayList.Synchronized(new ArrayList());
            if (separator != null)
            {
                _separators.Add(separator);
            }
        }

        public static implicit operator StringSeparator(string separator)
            => new StringSeparator(separator);

        public static StringSeparator operator +(StringSeparator separator, string str)
        {
            if (separator == null)
            {
                return new StringSeparator(str);
            }

            separator._separators.Add(str);
            return separator;
        }

        public string First => _separators.Count == 0 ? "," : ((string)_separators[0] ?? ",");
    }
}

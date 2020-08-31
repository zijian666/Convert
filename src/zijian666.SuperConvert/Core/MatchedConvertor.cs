using System;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Core
{
    public readonly struct MatchedConvertor<T> : IEquatable<MatchedConvertor<T>>, IComparable<MatchedConvertor<T>>
    {
        public MatchedConvertor(IConvertor<T> convertor, uint priority, MacthedLevel macthedLevel)
        {
            MacthedLevel = macthedLevel;
            Priority = priority;
            Convertor = convertor ?? throw new ArgumentNullException(nameof(convertor));
        }

        MacthedLevel MacthedLevel { get; }

        public uint Priority { get; }

        public IConvertor<T> Convertor { get; }

        public override bool Equals(object obj) => obj is MatchedConvertor<T> m && Equals(m);

        public override int GetHashCode()
        {
            if (Convertor == null)
            {
                return 0;
            }
            return MacthedLevel.GetHashCode() & Priority.GetHashCode() & Convertor.GetHashCode();
        }

        public static bool operator ==(MatchedConvertor<T> left, MatchedConvertor<T> right) => left.Equals(right);

        public static bool operator !=(MatchedConvertor<T> left, MatchedConvertor<T> right) => !(left == right);

        public bool Equals(MatchedConvertor<T> m)
            => Equals(m.Convertor, Convertor)
            && m.MacthedLevel == MacthedLevel
            && m.Priority == Priority;

        public int CompareTo(MatchedConvertor<T> other)
        {
            if (Convertor is null)
            {
                return -1;
            }
            if (other.Convertor is null)
            {
                return 1;
            }
            var i = Priority.CompareTo(other.Priority);
            if (i == 0)
            {
                return MacthedLevel.CompareTo(other.MacthedLevel);
            }
            return i;
        }
    }
}

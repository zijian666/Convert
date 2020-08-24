using System.Collections.Generic;
using System.Linq;

namespace zijian666.SuperConvert.Extensions
{
    static class CollectionExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any();
        public static bool IsEmpty<T>(this ICollection<T> enumerable) => enumerable == null || enumerable.Count == 0;
    }
}

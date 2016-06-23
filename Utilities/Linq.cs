using System;
using System.Collections.Generic;
using System.Linq;

namespace OutlineWF.Utilities
{
    public static class Linq
    {
        public static TAccumulate AggregateTwo<TSource, TAccumulate>(
            this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TSource, TAccumulate> func)
        {
            var result = seed;
            var v1 = default(TSource);
            var v2 = default(TSource);

            foreach (var val in source)
            {
                if (EqualityComparer<TSource>.Default.Equals(v1, default(TSource)))
                {
                    v1 = val;
                }
                else if (EqualityComparer<TSource>.Default.Equals(v2, default(TSource)))
                {
                    v2 = val;
                    result = func(result, v1, v2);
                    v1 = default(TSource);
                    v2 = default(TSource);
                }
            }
            return result;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TAlex.WPF.Helpers
{
    public static class EnumerableExtensions
    {
        public static int Count(this IEnumerable source)
        {
            var col = source as ICollection;
            if (col != null) return col.Count;

            int count = 0;
            var e = source.GetEnumerator();
            while (e.MoveNext()) count++;

            return count;
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace TemplateV2.Common.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
        {
            return self.Select((item, index) => (item, index));
        }
    }
}

using System.Collections.Generic;

namespace Narratore.Helpers
{
    public static class CollectionsHelper
    {
        public static bool IsEmpty<T>(this IReadOnlyCollection<T> collection)
        {
            if (collection == null || collection.Count == 0)
                return true;

            return false;
        }
    }
}

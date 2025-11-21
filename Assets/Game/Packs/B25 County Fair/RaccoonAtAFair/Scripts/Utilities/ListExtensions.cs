using System.Collections.Generic;

namespace XiaoHuanXiong.Common
{
    public static class ListExtensions
    {
        public static int SafeCount<T>(this ICollection<T> source)
        {
            if (source == null)
            {
                return -1;
            }
            return source.Count;
        }
    }
}
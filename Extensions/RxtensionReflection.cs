using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Extensions
{
    public static class RxtensionReflection
    {
        public static string GetProperty<T> (this T item, string prop)
        {
            return item.GetType().GetProperty(prop).GetValue(item, null).ToString();
        }
    }
}

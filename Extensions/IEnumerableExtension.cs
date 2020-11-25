using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items, int selected)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetProperty("Name"),
                       Value = item.GetProperty("Id"),
                       Selected = item.GetProperty("Id").Equals(selected.ToString())
                   };
        }
    }
}

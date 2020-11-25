using BookShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.ViewModels
{
    public class ShopCardVM
    {
        public IEnumerable<ShopingCard> listCard { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}

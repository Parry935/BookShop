using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class ShopingCard
    {

        public ShopingCard()
        {
            Count = 1;        
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        [NotMapped]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int MenuItemId { get; set; }

        [NotMapped]
        [ForeignKey("MenuItemId")]
        public virtual MenuItem MenuItem { get; set; }


        [Range(1,int.MaxValue, ErrorMessage = "Wartość musi być równa lub większa od jedynki")]
        public int Count { get; set; }
        

    }
}

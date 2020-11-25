using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class OrderHeader
    {
        public OrderHeader()
        {
            this.OrderTotalPrice = 0;
        }


        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Display(Name ="Order Total")]
        public double OrderTotalPrice { get; set; }

        public string Status { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Adress { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string Comments { get; set; }
    }
}

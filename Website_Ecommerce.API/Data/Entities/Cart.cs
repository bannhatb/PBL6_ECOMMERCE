using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Website_Ecommerce.API.Data.Entities
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Product_detail")]
        public int ProductDetailId { get; set; }
        public ProductDetail ProductDetail { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public bool State {get; set; }
    }
}
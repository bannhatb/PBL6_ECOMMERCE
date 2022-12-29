using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Website_Ecommerce.API.Data.Entities
{
    public class ProductDetail
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string Size { get; set; }
        public string Color { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public double Price { get; set; }
        public double InitialPrice { get; set; }
        public int Saled {get; set; }
        public int Booked {get; set;}
        public ICollection<Cart> Carts { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Website_Ecommerce.API.Data.Entities
{
    public class VoucherProduct
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Shop")]
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public double Value { get; set; }
        public double MinPrice { get; set; }
        public int Amount { get; set; }
        public int Booked { get; set; }
        public int Sale { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime Expired { get; set; }
        public string Description { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
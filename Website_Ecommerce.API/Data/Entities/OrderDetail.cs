using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Website_Ecommerce.API.Data.Entities
{
    public class OrderDetail
    {
        [Key]
        [Column(Order = 1)]
        public int OrderId { get; set; }
        [Required]
        public int ShopId {get; set;}
        public Order Order { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ProductDetailId { get; set; }
        public ProductDetail ProductDetail { get; set; }
        [ForeignKey("VoucherShop")]
        public int? VoucherProductId { get; set; }
        public VoucherProduct VoucherProduct { get; set; } 
        [Required]
        public int Amount { get; set; }
        [Required]
        public double Price { get; set; }
        [MaxLength(256)]
        public string Note { get; set; }
        public DateTime? ShopSendDate { get; set; }
        public DateTime? ShopConfirmDate { get; set; }
        public int State { get; set; }
        

    }
}
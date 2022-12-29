using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website_Ecommerce.API.Data.Entities
{
    public class VoucherOrder
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public double MinPrice { get; set; }
        [Required]
        public int Amount { get; set; }
        public int Booked { get; set; }
        public int Sale { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime Expired { get; set; }
        public string Description { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Website_Ecommerce.API.Data.Entities
{
    public class ProductCategory
    {
        [Key]
        [Column(Order = 1)]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        [Key]
        [Column(Order = 2)]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
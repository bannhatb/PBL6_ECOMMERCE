using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website_Ecommerce.API.Data.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }
        [Required]
        public DateTime DateCreate { get; set; }
        [Required]
        public int CreateBy { get; set; }
        public IList<ProductCategory> ProductCategories { get; set; }

    }
}
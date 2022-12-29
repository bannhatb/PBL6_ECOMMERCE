using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website_Ecommerce.API.Data.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Material { get; set; }
        [Required]
        public string Origin { get; set; }
        [Required]
        public string Description { get; set; }
        [ForeignKey("Shop")]
        public int ShopId { get; set; }
        public Shop Shop { get; set;}
        [Required]
        public bool Status { get; set; }
        [Required]
        public int TotalRate { get; set; }
        [Required]
        public float AverageRate { get; set; }
        public int Saled { get; set; }
        public ICollection<ProductDetail> ProductDetails { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public IList<ProductCategory> ProductCategories { get; set;}        
        

    }
}
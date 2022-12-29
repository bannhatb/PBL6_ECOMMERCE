namespace Website_Ecommerce.API.ModelQueries
{
    public class OrderDetailShowbyShop
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int Amount { get; set; }
        public DateTime? CreateAt { get; set; }
        public double Price { get; set; }
        public double DiscountShop { get; set; }
        public int Sate {get; set;}
        public string Note { get; set; }
        public DateTime? ConfirmAt {get; set;}
    }
}
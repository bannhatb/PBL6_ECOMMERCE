namespace Website_Ecommerce.API.ModelQueries
{
    public class ItemCartQueryModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int IdShop { get; set; }
        public int IdProductDetail { get; set; }
        public string NameProduct { get; set; }
        public double InitialPrice { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public int State { get; set; }

        public string UrlImage {get; set;}
    }
}
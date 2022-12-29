namespace Website_Ecommerce.API.ModelQueries
{
    public class VoucherProductQueryModel
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public double MinPrice { get; set; }
        public int Amount { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime Expired { get; set; }
        public string Description { get; set; }
    }
}
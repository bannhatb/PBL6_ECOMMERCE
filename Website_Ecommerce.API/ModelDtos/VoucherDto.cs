namespace Website_Ecommerce.API.ModelDtos
{
    public class VoucherOrderDto
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public double MinPrice { get; set; }
        public int Amount { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime Expired { get; set; }
        public bool State { get; set; }
    }
    
    public class VoucherShopDto
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public double Value { get; set; }
        public double MinPrice { get; set; }
        public int Amount { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime Expired { get; set; }
        public bool State { get; set; }

    }
}
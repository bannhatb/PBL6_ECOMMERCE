namespace Website_Ecommerce.API.ModelDtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int State { get; set; }
        public string Address { get; set; }
        public DateTime CreateDate { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public int UserId { get; set; }
        public int? VoucherId { get; set; }
        public int paymentMethodId { get; set; }
        public double totalPrice { get; set; }
        public IList<ItemOrderDto> ItemOrderDtos { get; set; }


        // <summary>
        // Map from Model Order to Order Dto
        // </summary>
        // public OrderDto ToOrderDto(Order order){
        //     var orderDto = new OrderDto{
        //         Id = order.Id,
        //         UserId = order.UserId,
        //         VoucherId = order.VoucherId,
        //         State = order.State,
        //         Address = order.Address,
        //         CreateDate = order.CreateDate,
        //         ShopSendDate = order.ShopSendDate
        //     };
        //     foreach (var i in order.OrderDetails)
        //     {
        //         orderDto.ItemOrderDtos.Add(new ItemOrderDto{
        //             ProductDetailId = i.ProductDetailId,
        //             VoucherProductId = i.VoucherProductId,
        //             Amount = i.Amount,
        //             Price = i.Price,
        //             Note = i.Note
        //         });
        //     }
        //     return orderDto;
        // }
    }


    public class ItemOrderDto
    {
        public int OrderId { get; set; }
        public int ProductDetailId { get; set; }
        public int? VoucherProductId { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public string Note { get; set; }
    }

    public class ShopStatisticProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int Sold { get; set; }
        public int Booked { get; set; }
        public int Inventory { get; set; }

    }

    // public class ShopViewOrderDetailDto
    // {
    //     public int OrderId { get; set; }
    //     public int ProductDetailId { get; set; }
    //     public int? VoucherProductId { get; set; }
    //     public int Amount { get; set; }
    //     public double Price { get; set; }
    //     public string Note { get; set; }
    //     public DateTime? ShopSendDate { get; set; }
    //     public DateTime? ShopConfirmDate { get; set; }
    // }
    public class ViewOrderState
    {
        public int IdShop;
        public string Name;
        public string Size;
        public string Color;
        public int Amount;
        public double Price;

    }

}

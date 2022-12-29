namespace Website_Ecommerce.API.Enum
{
    public enum StateOrderEnum
    {
        SENT = 1,
        CONFIRMED = 2,
        SHOP_SENT = 3,
        ARRIVED_WAREHOUSE = 4,
        DELIVERING = 5,
        RECEIVED = 6,
        CANCALLED = 0
    }
    public enum StateOrderDetailEnum{
        
        CANCALLED = 0,
        UNCONFIRMED = 1,
        CONFIRMED = 3
    }
}
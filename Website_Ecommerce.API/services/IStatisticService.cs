using Website_Ecommerce.API.ModelDtos;

namespace Website_Ecommerce.API.services
{
    public interface IStatisticService
    {
        Task<IList<ShopStatisticProductDto>> StatisticProduct(int idShop);
    }
}
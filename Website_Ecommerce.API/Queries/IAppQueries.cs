using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Queries
{
    public interface IAppQueries
    {
        #region Product
        Task<List<ProductQueryModel>> GetListProductByShopId();
        #endregion
    }
}
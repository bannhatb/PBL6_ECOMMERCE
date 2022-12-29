using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _dataContext;
        public IUnitOfWork UnitOfWork => _dataContext;

        public IQueryable<Cart> Carts => _dataContext.Carts;

        public CartRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Cart item)
        {
            _dataContext.Carts.Add(item);
        }

        public void Delete(Cart item)
        {
            _dataContext.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Update(Cart item)
        {
            _dataContext.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        /// <summary>
        /// Get all item by userId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<ItemCartQueryModel>> GetAllItemByIdUser(int id)
        {   
            var data = await _dataContext.Carts.Where(x => x.UserId == id && x.State == true)
                                        .Join(_dataContext.ProductDetails,
                                        cart => cart.ProductDetailId,
                                        productDetail => productDetail.Id,
                                        (cart, productDetail) => new { cart, productDetail })
                                        .Join(_dataContext.Products,
                                        cartProductDetail => cartProductDetail.productDetail.ProductId,
                                        product => product.Id,
                                        (cartProductDetail, product) => new
                                        {
                                            id = cartProductDetail.cart.Id,
                                            nameProduct = product.Name,
                                            idProductDetail = cartProductDetail.productDetail.Id,
                                            idShop = product.ShopId,
                                            userId = cartProductDetail.cart.UserId,
                                            initialPrice = cartProductDetail.productDetail.InitialPrice,
                                            price = cartProductDetail.productDetail.Price,
                                            amount = cartProductDetail.cart.Amount,
                                            
                                        })
                                        .Join(_dataContext.ProductImages, 
                                        itemCart => itemCart.idProductDetail, 
                                        img => img.ProductDetailId,
                                        (itemCart, img) => new ItemCartQueryModel{
                                            Id = itemCart.id,
                                            NameProduct = itemCart.nameProduct,
                                            IdProductDetail = itemCart.idProductDetail,
                                            IdShop = itemCart.idShop,
                                            UserId = itemCart.userId,
                                            InitialPrice = itemCart.initialPrice,
                                            Price = itemCart.price,
                                            Amount = itemCart.amount,
                                            UrlImage = img.UrlImage
                                        })
                                        .ToListAsync();
            return data;

        }
    }
}
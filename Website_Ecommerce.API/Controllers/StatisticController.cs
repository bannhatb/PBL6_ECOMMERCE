using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.Repositories;
using Website_Ecommerce.API.Response;
using Website_Ecommerce.API.services;

namespace Website_Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "MyAuthKey")]
    public class StatisticController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IShopRepository _shopRepository;
        private readonly IStatisticService _statisticService;

        public StatisticController(
            IOrderRepository orderRepository,
            IHttpContextAccessor httpContext,
            IShopRepository shopRepository,
            IStatisticService statisticService)
        {
            _orderRepository = orderRepository;
            _shopRepository = shopRepository;
            _httpContext = httpContext;
            _statisticService = statisticService;
        }

        /// <summary>
        /// Statistic turnover of days
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        [HttpGet("statistic-turnover-of-days")]
        public async Task<IActionResult> StatisticTurnoverOfDays(DateTime Start, DateTime End)
        {
            Dictionary<string, double> data = new Dictionary<string, double>();
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);
            var shopId = shop.Id;
            TimeSpan timeSpan = new TimeSpan(1, 0, 0, 0);

            IList<OrderDetail> orderDetails = await _orderRepository
                                            .OrderDetails
                                            .Where(o => o.ShopId == shopId && o.State == 3)
                                            .OrderByDescending(x => x.ShopSendDate)
                                            .ToListAsync();

            while (Start <= End)
            {
                var totalTurnover = 0.0;
                foreach (var i in orderDetails)
                {
                    if (DateOnly.FromDateTime(i.ShopConfirmDate.Value) == DateOnly.FromDateTime(Start))
                    {
                        totalTurnover = totalTurnover += i.Price;
                    }
                }
                data.Add(DateOnly.FromDateTime(Start).ToString(), totalTurnover);
                Start = Start.Add(timeSpan);
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.BadRequest,
                Result = new ResponseDefault()
                {
                    Data = data
                }
            });
        }

        /// <summary>
        /// Statistic turnover of months
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        [HttpGet("statistic-turnover-of-months")]
        public async Task<IActionResult> StatisticTurnoverOfMonths(DateTime Start, DateTime End)
        {
            Dictionary<string, double> data = new Dictionary<string, double>();
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);
            var shopId = shop.Id;

            IList<OrderDetail> orderDetails = await _orderRepository
                                            .OrderDetails
                                            .Where(o => o.ShopId == shopId && o.State == 3/*(int)StateOrderEnum.RECEIVED%*/)
                                            .OrderByDescending(x => x.ShopSendDate)
                                            .ToListAsync();

            while (Start <= End)
            {
                var totalTurnover = 0.0;
                foreach (var i in orderDetails)
                {
                    if (DateOnly.FromDateTime(i.ShopConfirmDate.Value) == DateOnly.FromDateTime(Start))
                    {
                        totalTurnover = totalTurnover += i.Price;
                    }
                }
                data.Add(DateOnly.FromDateTime(Start).ToString(), totalTurnover);
                Start = Start.AddMonths(1);
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.BadRequest,
                Result = new ResponseDefault()
                {
                    Data = data
                }
            });
        }

        /// <summary>
        /// Statistic product
        /// </summary>
        /// <returns></returns>
        [HttpGet("statistic-product")]
        public async Task<IActionResult> StatisticProduct()
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);
            var shopId = shop.Id;

            var data = await _statisticService.StatisticProduct(shopId);
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.BadRequest,
                Result = new ResponseDefault()
                {
                    Data = data
                }
            });
        }


    }
}
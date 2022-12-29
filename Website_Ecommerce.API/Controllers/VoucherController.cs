using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelDtos;
using Website_Ecommerce.API.Repositories;
using Website_Ecommerce.API.Response;

namespace Website_Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherOrderRepository _voucherOrderRepository;
        private readonly IVoucherProductRepository _voucherProductRepository;
        private readonly IMapper _mapper;
        public VoucherController(
            IVoucherOrderRepository voucherOrderRepository,
            IVoucherProductRepository voucherProductRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _voucherOrderRepository = voucherOrderRepository;
            _voucherProductRepository = voucherProductRepository;
        }

        #region Voucher Order
        /// <summary>
        /// Add voucher by admin
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("add-voucher-by-admin")]
        public async Task<IActionResult> Add([FromBody] VoucherOrderDto request, CancellationToken cancellationToken)
        {
            var item = _mapper.Map<VoucherOrder>(request);
            _voucherOrderRepository.Add(item);
            var result = await _voucherOrderRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Add ProductImage fail"
                    }
                });
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Add ProductImage success"
                }
            });
        }

        /// <summary>
        /// Update voucher by admin
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("update-voucher-by-admin")]
        public async Task<IActionResult> Update([FromBody] VoucherOrderDto request, CancellationToken cancellationToken)
        {
            var item = _mapper.Map<VoucherOrder>(request);
            _voucherOrderRepository.Update(item);
            var result = await _voucherOrderRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Update Voucher Order fail"
                    }
                });
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Update Voucher Order success"
                }
            });
        }

        /// <summary>
        /// Delete voucher by admin follow id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete-Voucher-by-admin/{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            VoucherOrder voucherOrder = await _voucherOrderRepository.VoucherOrders.FirstOrDefaultAsync(p => p.Id == id);

            if (voucherOrder == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "Not Found Vouchers"
                    }
                });
            }
            voucherOrder.Amount = 0;
            _voucherOrderRepository.Update(voucherOrder);
            var result = await _voucherOrderRepository.UnitOfWork.SaveAsync(cancellationToken);

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = voucherOrder.Id.ToString()
                    }
                });
            }
            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Update Voucher fail"
                }
            });
        }

        /// <summary>
        /// Get voucher order by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-voucher-order-by/{id}")]
        public async Task<IActionResult> GetVoucherOrderById(int id)
        {
            var voucherOrder = await _voucherOrderRepository.VoucherOrders.FirstOrDefaultAsync(p => p.Id == id);
            if (voucherOrder is null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "Not found Voucher"
                    }
                });
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = voucherOrder
                }
            });
        }

        /// <summary>
        /// Get all voucher order by admin
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-voucher-order-by-admin")]
        public async Task<IActionResult> GetAllVoucherOrder()
        {
            var vouchersUnexpired = await _voucherOrderRepository.VoucherOrders.Where(x => x.Expired > DateTime.Now).ToListAsync();
            if (vouchersUnexpired.Count() == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "Not found Voucher"
                    }
                });
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = vouchersUnexpired
                }
            });
        }

        /// <summary>
        /// Get voucher order by time
        /// </summary>
        /// <param name="timeIn"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>Get voucher order by time
        [HttpGet("get-voucher-order-by-time")]
        public async Task<IActionResult> GetVoucherOrderByTime(DateTime timeIn, DateTime timeOut)
        {
            var listVoucher = await _voucherOrderRepository.GetAllVoucherByDate(timeIn, timeOut);
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = listVoucher
                }
            });
        }

        /// <summary>
        /// Get voucher availablility
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-voucher-availability")]
        public async Task<IActionResult> GetVoucherAvailability()
        {
            var vouchers = await _voucherOrderRepository.GetAllVoucherMatch();
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = vouchers
                }
            });

        }
        #endregion

        #region Voucher Product

        /// <summary>
        /// Get list voucher by shopId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-voucher-product-by/{id}")]
        public async Task<IActionResult> GetListVoucherProductByShopId(int id)
        {
            var vouchers = await _voucherProductRepository.GetListVoucherProductById(id);
            if (vouchers is null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "NotFound Voucher of shop"
                    }
                });
            }

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = vouchers
                }
            });

        }

        #endregion

    }
}
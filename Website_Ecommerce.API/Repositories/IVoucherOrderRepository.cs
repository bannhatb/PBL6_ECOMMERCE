using Website_Ecommerce.API.Data.Entities;

namespace Website_Ecommerce.API.Repositories
{
    public interface IVoucherOrderRepository : IRepository<VoucherOrder>
    {
        void Add(VoucherOrder voucherOrder);
        void Update(VoucherOrder voucherOrder);
        IQueryable<VoucherOrder> VoucherOrders { get; }

        /// <summary>
        /// Get all voucher by date
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<IEnumerable<VoucherOrder>> GetAllVoucherByDate(DateTime start, DateTime end);
        // Task<IList<VoucherOrder>> GetAllVoucherbyCheckVoucher(Order order);

        /// <summary>
        /// Get all voucher match
        /// </summary>
        /// <returns></returns>
        Task<List<VoucherOrder>> GetAllVoucherMatch();
    }
}
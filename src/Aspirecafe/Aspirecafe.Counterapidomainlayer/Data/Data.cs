using AspireCafe.CounterApiDomainLayer.Managers.Context;
using AspireCafe.CounterApiDomainLayer.Managers.Models.Domain;
using AspireCafe.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace AspireCafe.CounterApiDomainLayer.Data
{
    public class Data:IData
    {
        private readonly CounterContext _context;

        public Data(CounterContext context)
        {
            _context = context;
        }

        public async Task<OrderDomainModel> GetOrderAsync(Guid orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            return order;
        }

        public async Task<OrderDomainModel> SubmitOrderAsync(OrderDomainModel order)
        {
            order.DocumentType = DocumentType.Order.ToString();
            order.Id = Guid.NewGuid();
            order.CreatedDate = DateTime.UtcNow;
            order.ModifiedDate = DateTime.UtcNow;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}

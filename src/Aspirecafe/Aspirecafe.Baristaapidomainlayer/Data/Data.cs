using AspireCafe.BaristaApiDomainLayer.Managers.Context;
using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace AspireCafe.BaristaApiDomainLayer.Data
{
    public class Data : IData
    {
        private readonly BaristaContext _context;

        public Data(BaristaContext context)
        {
            _context = context;
        }

        public async Task<ProcessingOrderDomainModel> AddOrderAsync(ProcessingOrderDomainModel order)
        {
            order.DocumentType = DocumentType.BaristaOrder.ToString();
            order.CreatedDate = DateTime.UtcNow;
            order.Id = Guid.NewGuid();
            order.History = new List<OrderProcessingStatusHistory>() { new OrderProcessingStatusHistory() { CurrentStation = OrderProcessStation.Bar, EntryTime = DateTime.UtcNow } };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<ProcessingOrderDomainModel>> FetchActiveOrdersAsync()
        {
            return await _context.Orders.Where(w => w.OrderStatus != OrderStatus.Cancel || w.OrderStatus != OrderStatus.Delivered).ToListAsync();
        }

        public async Task<ProcessingOrderDomainModel> UpdateOrderStatusAsync(Guid orderId, OrderProcessStation orderProcessStation, OrderProcessStatus orderProcessStatus)
        {
            //fetch current status
            var current = await _context.Orders.FirstOrDefaultAsync(f => f.OrderId == orderId);
            //update current history record
            current.History.LastOrDefault().ExitTime = DateTime.UtcNow;
            //prepare history record
            OrderProcessingStatusHistory history = new OrderProcessingStatusHistory()
            {
                CurrentStation = orderProcessStation,
                PreviousStation = current.CurrentStation,
                EntryTime = DateTime.UtcNow
            };
            current.History.Add(history);
            current.CurrentStation = orderProcessStation;
            current.ProcessStatus = orderProcessStatus;
            current.ModifiedDate = DateTime.UtcNow;
            _context.Update(current);
            await _context.SaveChangesAsync();
            return current;
        }
    }
}

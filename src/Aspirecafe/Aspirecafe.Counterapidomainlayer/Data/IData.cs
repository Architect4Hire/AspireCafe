using AspireCafe.CounterApiDomainLayer.Managers.Models.Domain;
using AspireCafe.Shared.Enums;

namespace AspireCafe.CounterApiDomainLayer.Data
{
    public interface IData
    {
        Task<OrderDomainModel> SubmitOrderAsync(OrderDomainModel order);
        Task<OrderDomainModel> GetOrderAsync(Guid orderId);
        Task<OrderDomainModel> UpdateOrderAsync(OrderDomainModel order);
        Task<bool> PayOrderAsync(Guid orderId, PaymentMethod paymentMethod, decimal amount, decimal tip);
    }
}

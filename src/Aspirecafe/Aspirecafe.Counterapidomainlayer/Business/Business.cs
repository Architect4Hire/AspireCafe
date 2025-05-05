using AspireCafe.CounterApiDomainLayer.Data;
using AspireCafe.CounterApiDomainLayer.Managers.Extensions;
using AspireCafe.CounterApiDomainLayer.Managers.Models.Service;
using AspireCafe.CounterApiDomainLayer.Managers.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Business
{
    public class Business : IBusiness
    {
        private readonly IData _data;

        public Business(IData data)
        {
            _data = data;
        }

        public async Task<OrderServiceModel> GetOrderAsync(Guid orderId)
        {
            var model = await _data.GetOrderAsync(orderId);
            return model.MapToServiceModel();
        }

        public async Task<OrderServiceModel> PayOrderAsync(OrderPaymentViewModel model)
        {
            var data = await _data.PayOrderAsync(model.OrderId, model.PaymentMethod, model.CheckAmount, model.TipAmount);
            if (data)
            {
                var order = await _data.GetOrderAsync(model.OrderId);
                return order.MapToServiceModel();
            }
            return null;
        }

        public async Task<OrderServiceModel> SubmitOrderAsync(OrderViewModel order)
        {
            var model = await _data.SubmitOrderAsync(order.MapToDomainModel());
            return model.MapToServiceModel();
        }

        public async Task<OrderServiceModel> UpdateOrderAsync(OrderViewModel order)
        {
            var model = await _data.UpdateOrderAsync(order.MapToDomainModel());
            return model.MapToServiceModel();
        }
    }
}

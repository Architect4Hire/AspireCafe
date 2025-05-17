using AspireCafe.CounterApiDomainLayer.Data;
using AspireCafe.CounterApiDomainLayer.Managers.Extensions;
using AspireCafe.Shared.HttpClients;
using AspireCafe.Shared.Models.Service.Counter;
using AspireCafe.Shared.Models.View.Counter;
using AspireCafe.Shared.Models.View.Product;

namespace AspireCafe.CounterApiDomainLayer.Business
{
    public class Business : IBusiness
    {
        private readonly IData _data;
        private readonly IProductHttpClient _productClient;

        public Business(IData data, IProductHttpClient product)
        {
            _data = data;
            _productClient = product;
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
            var enrinchedData = await _productClient.FetchMetadata(new ProductMetaDataViewModel() { ProductIds = model.LineItems.Select(s => s.ProductId).ToList() });
            var baristaItems = new List<Guid>();
            var kitchenItems = new List<Guid>();
            foreach (var item in enrinchedData.Data.Metadata)
            {
                if(item.Value == Shared.Enums.RouteType.Barista)
                {
                    baristaItems.Add(item.Key);
                }

                if (item.Value == Shared.Enums.RouteType.Kitchen)
                {
                    kitchenItems.Add(item.Key);
                }
            }
            // if barista send to the barista api with barista product types
            // if kitchen send to the kitchen api with kitchen product types
            return model.MapToServiceModel();
        }

        public async Task<OrderServiceModel> UpdateOrderAsync(OrderViewModel order)
        {
            var model = await _data.UpdateOrderAsync(order.MapToDomainModel());
            return model.MapToServiceModel();
        }
    }
}

using AspireCafe.CounterApiDomainLayer.Data;
using AspireCafe.CounterApiDomainLayer.Managers.Extensions;
using AspireCafe.CounterApiDomainLayer.Managers.Models.Domain;
using AspireCafe.Shared.Enums;
using AspireCafe.Shared.HttpClients;
using AspireCafe.Shared.Models.Message.Shared;
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
            var domainModel = await _data.SubmitOrderAsync(order.MapToDomainModel());
            var lineItems = domainModel.LineItems ?? new List<OrderLineItemDomainModel>();
            var productIds = lineItems.Select(li => li.ProductId).Distinct().ToList();
            var metaDataResult = await _productClient.FetchMetadata(
                new ProductMetaDataViewModel { ProductIds = productIds }
            );
            var metadata = metaDataResult?.Data?.Metadata ?? new Dictionary<Guid, RouteType>();
            // Group line items by ProductId for efficient lookup
            var lineItemsByProductId = lineItems
                .GroupBy(li => li.ProductId)
                .ToDictionary(g => g.Key, g => g.ToList());
            var barista = new List<ProductInfoMessageModel>();
            var kitchen = new List<ProductInfoMessageModel>();
            foreach (var kvp in metadata)
            {
                if (!lineItemsByProductId.TryGetValue(kvp.Key, out var items))
                    continue;

                var targetList = kvp.Value == RouteType.Barista ? barista :
                                 kvp.Value == RouteType.Kitchen ? kitchen : null;

                if (targetList != null)
                {
                    targetList.AddRange(items.Select(li => new ProductInfoMessageModel
                    {
                        ProductName = li.ProductName,
                        Notes = li.Notes
                    }));
                }
            }
            // TODO: Send barista and kitchen lists to their respective APIs if needed
            return domainModel.MapToServiceModel();
        }

        public async Task<OrderServiceModel> UpdateOrderAsync(OrderViewModel order)
        {
            var model = await _data.UpdateOrderAsync(order.MapToDomainModel());
            return model.MapToServiceModel();
        }
    }
}

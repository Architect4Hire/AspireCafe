using AspireCafe.CounterApiDomainLayer.Data;
using AspireCafe.CounterApiDomainLayer.Managers.Extensions;
using AspireCafe.CounterApiDomainLayer.Managers.Models.Domain;
using AspireCafe.Shared.Enums;
using AspireCafe.Shared.HttpClients;
using AspireCafe.Shared.Models.Message.Barista;
using AspireCafe.Shared.Models.Message.Kitchen;
using AspireCafe.Shared.Models.Message.Shared;
using AspireCafe.Shared.Models.Service.Counter;
using AspireCafe.Shared.Models.View.Counter;
using AspireCafe.Shared.Models.View.Product;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace AspireCafe.CounterApiDomainLayer.Business
{
    public class Business : IBusiness
    {
        private readonly IData _data;
        private readonly IProductHttpClient _productClient;
        private ServiceBusClient _serviceBusClient;

        public Business(IData data, IProductHttpClient product, IConfiguration config)
        {
            _data = data;
            _productClient = product;
            _serviceBusClient = new ServiceBusClient(config.GetConnectionString("serviceBusConnection"));
        }

        public async Task<OrderServiceModel> GetOrderAsync(Guid orderId)
        {
            //var model = await _data.GetOrderAsync(orderId);
            //return model.MapToServiceModel();
            var items = new List<ProductInfoMessageModel>();
            items.Add(new ProductInfoMessageModel
            {
                ProductName = "Espresso",
                Notes = "Extra hot"
            });
            await SendOrderToServiceBusAsync("barista", items,
                () => new BaristaOrderMessageModel
                {
                    CustomerName = "Robert",
                    TableNumber = 1,
                    Items = items,
                    RouteType = RouteType.Barista
                });
            return new OrderServiceModel(); // Placeholder return statement
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
            //send to service bus for background processes
            await SendOrderToServiceBusAsync("barista", barista,
                () => new BaristaOrderMessageModel
                {
                    CustomerName = domainModel.Header.CustomerName,
                    TableNumber = domainModel.Header.TableNumber.GetValueOrDefault(),
                    Items = barista,
                    RouteType = RouteType.Barista
                });

            await SendOrderToServiceBusAsync("kitchen", kitchen,
                () => new KitchenOrderMessageModel
                {
                    CustomerName = domainModel.Header.CustomerName,
                    TableNumber = domainModel.Header.TableNumber.GetValueOrDefault(),
                    Items = kitchen,
                    RouteType = RouteType.Kitchen
                });
            // TODO: Send barista and kitchen lists to their respective APIs if needed
            return domainModel.MapToServiceModel();
        }

        public async Task<OrderServiceModel> UpdateOrderAsync(OrderViewModel order)
        {
            var model = await _data.UpdateOrderAsync(order.MapToDomainModel());
            return model.MapToServiceModel();
        }

        #region private methods

        private async Task SendOrderToServiceBusAsync<T>(string topicName,List<ProductInfoMessageModel> items,Func<T> messageFactory)
        {
            if (items == null || items.Count == 0)
                return;

            await using var sender = _serviceBusClient.CreateSender("purchased-orders");
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            var message = new ServiceBusMessage(JsonSerializer.Serialize(messageFactory()));

            if (!messageBatch.TryAddMessage(message))
                throw new Exception($"Couldn't route order to the service bus subscription - {topicName}");

            try
            {
                await sender.SendMessagesAsync(messageBatch);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception($"Failed to send message batch to {topicName}: {ex.Message}", ex);
            }
        }

        #endregion
    }
}

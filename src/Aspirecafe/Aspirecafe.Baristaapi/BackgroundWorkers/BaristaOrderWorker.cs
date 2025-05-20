using AspireCafe.BaristaApiDomainLayer.Facade;
using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Domain.Orders;
using AspireCafe.Shared.Models.Message.Barista;
using Azure.Messaging.ServiceBus;
using System.Text.Json;

namespace AspireCafe.BaristaApi.BackgroundWorkers
{
    public class BaristaOrderWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ILogger<BaristaOrderWorker> _logger;
        private readonly string _topicName = "purchased-orders";
        private readonly string _subscriptionName = "barista-orders";

        public BaristaOrderWorker(IServiceProvider serviceProvider, ServiceBusClient serviceBusClient, ILogger<BaristaOrderWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _serviceBusClient = serviceBusClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var processor = _serviceBusClient.CreateProcessor(_topicName, _subscriptionName, new ServiceBusProcessorOptions());
            processor.ProcessMessageAsync += ProcessMessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;
            await processor.StartProcessingAsync(stoppingToken);
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
            finally
            {
                await processor.StopProcessingAsync(stoppingToken);
                await processor.DisposeAsync();
            }
        }

        private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();
                var message = JsonSerializer.Deserialize<BaristaOrderMessageModel>(body);
                if (message != null && message.RouteType == RouteType.Barista)
                {
                    var domainModel = MapToDomainModel(message);
                    using var scope = _serviceProvider.CreateScope();
                    var facade = scope.ServiceProvider.GetRequiredService<IFacade>();
                    await facade.AddOrderAsync(domainModel);
                    await args.CompleteMessageAsync(args.Message);
                }
                else
                {
                    // Ignore non-Barista orders, just complete the message
                    await args.CompleteMessageAsync(args.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing barista order message");
                await args.AbandonMessageAsync(args.Message);
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Service Bus error");
            return Task.CompletedTask;
        }

        private ProcessingOrderDomainModel MapToDomainModel(BaristaOrderMessageModel message)
        {
            return new ProcessingOrderDomainModel
            {
                OrderId = message.OrderId,
                CurrentStation = OrderProcessStation.Bar,
                OrderStatus = OrderStatus.Pending,
                ProcessStatus = OrderProcessStatus.Waiting,
                CustomerName = message.CustomerName,
                TableNumber = message.TableNumber,
                Items = message.Items
        .Select(item => new OrderProcessingLineItem
        {
            ProductName = item.ProductName,
            Notes = item.Notes ?? string.Empty
        })
        .ToList(),
            };
        }
    }
}

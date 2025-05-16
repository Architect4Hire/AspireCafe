using AspireCafe.CounterApiDomainLayer.Managers.Models.Domain;
using AspireCafe.Shared.Models.Service.Counter;
using AspireCafe.Shared.Models.View.Counter;

namespace AspireCafe.CounterApiDomainLayer.Managers.Extensions
{
    internal static class CounterExtensions
    {
        #region ViewModel -> DomainModel Mappers

        public static OrderDomainModel MapToDomainModel(this OrderViewModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new OrderDomainModel
            {
                OrderId = model.OrderId == null ? Guid.NewGuid() : (Guid)model.OrderId,
                DocumentType = nameof(OrderDomainModel),
                Header = model.MapHeaderToDomainModel(),
                LineItems = model.Items.Select(x => x.MapItemsToDomainModel()).ToList(),
                Footer = model.MapFooterToDomainModel()
            };
        }

        public static OrderHeaderDomainModel MapHeaderToDomainModel(this OrderViewModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new OrderHeaderDomainModel
            {
                OrderType = model.OrderType,
                TableNumber = model.TableNumber,
                CustomerName = model.CustomerName
            };
        }

        public static OrderFooterDomainModel MapFooterToDomainModel(this OrderViewModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new OrderFooterDomainModel
            {
                Notes = model.Notes,
                SubTotal = model.SubTotal,
                Tax = model.Tax,
                Total = model.Total,
                PaymentMethod = model.PaymentMethod,
                PaymentStatus = model.PaymentStatus,
                OrderStatus = model.OrderStatus
            };
        }

        public static OrderLineItemDomainModel MapItemsToDomainModel(this LineItemViewModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new OrderLineItemDomainModel
            {
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                Quantity = model.Quantity,
                Price = model.Price,
                Notes = model.Notes
            };
        }

        #endregion

        #region DomainModel -> ServiceModel Mappers

        public static OrderServiceModel MapToServiceModel(this OrderDomainModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new OrderServiceModel
            {
                Header = model.Header.MapToServiceModel(),
                Lines = model.LineItems.Select(x => x.MapToServiceModel()).ToList(),
                Footer = model.Footer.MapToServiceModel()
            };
        }

        public static OrderHeaderServiceModel MapToServiceModel(this OrderHeaderDomainModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new OrderHeaderServiceModel
            {
                OrderType = model.OrderType.ToString(),
                TableNumber = model.TableNumber,
                CustomerName = model.CustomerName
            };
        }

        public static OrderLineItemServiceModel MapToServiceModel(this OrderLineItemDomainModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new OrderLineItemServiceModel
            {
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                Quantity = model.Quantity,
                Price = model.Price,
                Notes = model.Notes
            };
        }

        public static OrderFooterServiceModel MapToServiceModel(this OrderFooterDomainModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new OrderFooterServiceModel
            {
                Notes = model.Notes,
                SubTotal = model.SubTotal,
                Tax = model.Tax,
                Total = model.Total,
                PaymentMethod = model.PaymentMethod.ToString(),
                PaymentStatus = model.PaymentStatus.ToString(),
                OrderStatus = model.OrderStatus.ToString()
            };
        }

        #endregion
    }
}

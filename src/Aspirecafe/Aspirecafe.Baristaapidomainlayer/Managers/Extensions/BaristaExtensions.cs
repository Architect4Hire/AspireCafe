using AspireCafe.Shared.Models.Domain.Orders;
using AspireCafe.Shared.Models.Service.OrderUpdate;

namespace AspireCafe.BaristaApiDomainLayer.Managers.Extensions
{
    public static class BaristaExtensions
    {
        public static OrderUpdateServiceModel MapToServiceModel(this ProcessingOrderDomainModel model)
        {
            return new OrderUpdateServiceModel()
            {
                OrderId = model.OrderId,
                OrderStatus = model.OrderStatus,
                CookingStatus = model.ProcessStatus,
                Station = model.CurrentStation
            };
        }

        public static OrderGridServiceModel MapToServiceModel(this List<ProcessingOrderDomainModel> model)
        {
            return new OrderGridServiceModel();
        }
    }
}

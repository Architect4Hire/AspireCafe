using AspireCafe.Shared.Enums;

namespace AspireCafe.Shared.Models.Service.Product
{
    public class ProductMetaDataServiceModel: ServiceBaseModel
    {
        public Dictionary<Guid,RouteType> Metadata { get; set; } = new Dictionary<Guid, RouteType>();
    }
}

using AspireCafe.ProductApiDomainLayer.Managers.Models.Enums;
using AspireCafe.Shared.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.ProductApiDomainLayer.Managers.Models.Service
{
    public class ProductMetaDataServiceModel: ServiceBaseModel
    {
        public Dictionary<Guid,RouteType> Metadata { get; set; } = new Dictionary<Guid, RouteType>();
    }
}

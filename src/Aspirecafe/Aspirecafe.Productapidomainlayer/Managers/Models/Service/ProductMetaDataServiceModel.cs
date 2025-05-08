using AspireCafe.ProductApiDomainLayer.Managers.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.ProductApiDomainLayer.Managers.Models.Service
{
    public class ProductMetaDataServiceModel
    {
        public Guid ProductId { get; set; }
        public RouteType RouteType { get; set; }
    }
}

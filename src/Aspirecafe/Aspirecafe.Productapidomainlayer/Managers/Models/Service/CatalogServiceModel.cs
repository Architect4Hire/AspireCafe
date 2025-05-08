using AspireCafe.Shared.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.ProductApiDomainLayer.Managers.Models.Service
{
    public class CatalogServiceModel: ServiceBaseModel
    {
        public Dictionary<string, List<CatalogServiceModel>> Catalog { get; set; } = new Dictionary<string, List<CatalogServiceModel>>();
    }
}

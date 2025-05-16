using AspireCafe.Shared.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.Shared.Models.Service.Product
{
    public class CatalogServiceModel: ServiceBaseModel
    {
        public Dictionary<string, List<CatalogItemServiceModel>> Catalog { get; set; } = new Dictionary<string, List<CatalogItemServiceModel>>();
    }
}

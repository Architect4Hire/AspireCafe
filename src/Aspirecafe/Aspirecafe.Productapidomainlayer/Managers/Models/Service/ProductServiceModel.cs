using AspireCafe.ProductApiDomainLayer.Managers.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.ProductApiDomainLayer.Managers.Models.Service
{
    public class ProductServiceModel
    {
        public string ProductId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ProductType ProductType { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}

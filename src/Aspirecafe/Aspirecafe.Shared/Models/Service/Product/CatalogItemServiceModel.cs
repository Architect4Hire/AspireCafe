using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.Shared.Models.Service.Product
{
    public class CatalogItemServiceModel:ServiceBaseModel
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
        public string ProductCategory { get; set; } = string.Empty;
        public string ProductSubCategory { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; } = 0;

    }
}

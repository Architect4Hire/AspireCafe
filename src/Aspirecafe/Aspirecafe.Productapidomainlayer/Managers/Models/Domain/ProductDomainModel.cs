﻿using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Domain;

namespace AspireCafe.ProductApiDomainLayer.Managers.Models.Domain
{
    public class ProductDomainModel : DomainBaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ProductType ProductType { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string ProductSubCategory { get; set; } = string.Empty;
        public ProductStatus ProductStatus { get; set; }
        public RouteType RouteType { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
    }
}

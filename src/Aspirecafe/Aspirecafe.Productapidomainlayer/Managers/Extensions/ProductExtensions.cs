using AspireCafe.ProductApiDomainLayer.Managers.Models.Domain;
using AspireCafe.ProductApiDomainLayer.Managers.Models.Enums;
using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.ProductApiDomainLayer.Managers.Extensions
{
    public static class ProductExtensions
    {
        #region ViewModel -> DomainModel Mappers
        #endregion

        #region DomainModel -> ServiceModel Mappers 

        public static ProductMetaDataServiceModel MapToMetadataServiceModel(this List<ProductDomainModel> products)
        {
            if (products == null) throw new ArgumentNullException(nameof(products));
            var data = new Dictionary<Guid, RouteType>();
            foreach (var product in products)
            {
                data.Add(product.Id, product.RouteType);
            }
            return new ProductMetaDataServiceModel
            {
                Metadata = data
            };
        }

        public static CatalogServiceModel MapToServiceModel(this List<ProductDomainModel> products)
        {
            if (products == null) throw new ArgumentNullException(nameof(products));

            return new CatalogServiceModel
            {
                Catalog = products
                    .GroupBy(product => product.ProductCategory.ToString())
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(p => new CatalogItemServiceModel
                        {
                            ProductId = p.Id.ToString(),
                            ProductName = p.Name,
                            ProductDescription = p.Description,
                            ProductPrice = p.Price,
                            ProductCategory = p.ProductCategory.ToString(),
                            ProductImage = p.ImageUrl,
                            ProductSubCategory = p.ProductSubCategory
                        }).ToList()
                    )
            };
        }

        public static ProductServiceModel MapToServiceModel(this ProductDomainModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new ProductServiceModel
            {
                ProductId = model.Id.ToString(),
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ProductType = model.ProductType,
                ProductCategory = model.ProductCategory,
                ImageUrl = model.ImageUrl
            };
        }

        #endregion
    }
}

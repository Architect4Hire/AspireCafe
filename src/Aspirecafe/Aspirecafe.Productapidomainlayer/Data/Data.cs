using AspireCafe.ProductApiDomainLayer.Managers.Context;
using AspireCafe.ProductApiDomainLayer.Managers.Models.Domain;
using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;
using AspireCafe.ProductApiDomainLayer.Managers.Models.View;
using AspireCafe.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.ProductApiDomainLayer.Data
{
    public class Data : IData
    {
        private readonly ProductContext _context;
        public Data(ProductContext context)
        {
            _context = context;
        }

        public async Task<ProductDomainModel> CreateProductAsync(ProductDomainModel product)
        {
            product.Id = Guid.NewGuid();
            product.CreatedDate = DateTime.UtcNow;
            product.ModifiedDate = DateTime.UtcNow;
            product.DocumentType = DocumentType.Product.ToString();
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<ProductDomainModel> DeleteProductAsync(Guid productId)
        {
            //does not implement a physical delete only a soft delete
            var data = await FetchProductByIdAsync(productId);
            data.IsActive = false;
            data.IsAvailable = false;
            data.ModifiedDate = DateTime.UtcNow;
            _context.Products.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<ProductDomainModel> FetchProductByIdAsync(Guid productId)
        {
            return await _context.Products.FirstOrDefaultAsync(w => w.Id == productId && w.IsAvailable && w.IsActive);
        }

        public async Task<ProductDomainModel> UpdateProductAsync(ProductDomainModel product)
        {
            var data = await FetchProductByIdAsync(product.Id);
            data.ModifiedDate = DateTime.UtcNow;
            data.Name = product.Name;
            data.Description = product.Description;
            data.Price = product.Price;
            data.ProductCategory = product.ProductCategory;
            data.ProductSubCategory = product.ProductSubCategory;
            data.ProductStatus = product.ProductStatus;
            data.ProductSubCategory = product.ProductSubCategory;
            data.RouteType = product.RouteType;
            data.ImageUrl = product.ImageUrl;
            data.IsAvailable = product.IsAvailable;
            data.IsActive = product.IsActive;
            data.ProductType = product.ProductType;
            data.ModifiedDate = DateTime.UtcNow;
            _context.Products.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }
    }
}

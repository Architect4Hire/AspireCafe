using AspireCafe.ProductApiDomainLayer.Data;
using AspireCafe.ProductApiDomainLayer.Managers.Extensions;
using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;
using AspireCafe.ProductApiDomainLayer.Managers.Models.View;

namespace AspireCafe.ProductApiDomainLayer.Business
{
    public class Business : IBusiness
    {
        private readonly IData _data;

        public Business(IData data)
        {
            _data = data;
        }

        public async Task<ProductServiceModel> CreateProductAsync(ProductViewModel product)
        {
            var data = await _data.CreateProductAsync(product.MapToDomainModel());
            return data.MapToServiceModel();
        }

        public async Task<ProductServiceModel> DeleteProductAsync(Guid productId)
        {
            var data = await _data.DeleteProductAsync(productId);
            return data.MapToServiceModel();
        }

        public async Task<ProductServiceModel> FetchProductByIdAsync(Guid productId)
        {
            var data = await _data.FetchProductByIdAsync(productId);
            return data.MapToServiceModel();
        }

        public async Task<ProductServiceModel> UpdateProductAsync(ProductViewModel product)
        {
            var data = await _data.UpdateProductAsync(product.MapToDomainModel());
            return data.MapToServiceModel();
        }
    }
}

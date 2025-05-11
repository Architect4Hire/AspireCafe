using AspireCafe.ProductApiDomainLayer.Data;
using AspireCafe.ProductApiDomainLayer.Managers.Extensions;
using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;

namespace AspireCafe.ProductApiDomainLayer.Business
{
    public class Business : IBusiness
    {
        private readonly IData _data;

        public Business(IData data)
        {
            _data = data;
        }

        public async Task<ProductServiceModel> FetchProductByIdAsync(Guid productId)
        {
            var data = await _data.FetchProductByIdAsync(productId);
            return data.MapToServiceModel();
        }

    }
}

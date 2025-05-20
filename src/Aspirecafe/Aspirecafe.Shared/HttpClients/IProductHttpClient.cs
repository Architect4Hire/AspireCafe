using AspireCafe.Shared.Models.Service.Product;
using AspireCafe.Shared.Models.View.Product;
using AspireCafe.Shared.Results;
using Refit;

namespace AspireCafe.Shared.HttpClients
{
    public interface IProductHttpClient
    {
        [Post("/catalog/fetchmetadata")]
        Task<Result<ProductMetaDataServiceModel>> FetchMetadata(ProductMetaDataViewModel products);
    }
}

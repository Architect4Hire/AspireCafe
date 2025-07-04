﻿using AspireCafe.ProductApiDomainLayer.Business;
using AspireCafe.ProductApiDomainLayer.Managers.Validators;
using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Service.Product;
using AspireCafe.Shared.Models.View.Product;
using AspireCafe.Shared.Results;
using Microsoft.Extensions.Caching.Distributed;

namespace AspireCafe.ProductApiDomainLayer.Facade
{
    public class Facade : IFacade
    {
        private readonly IBusiness _business;
        private readonly ProductViewModelValidator _validator;

        public Facade(IBusiness business, IDistributedCache cache)
        {
            _business = business;
            _validator = new ProductViewModelValidator();
        }

        public async Task<Result<ProductServiceModel>> CreateProductAsync(ProductViewModel product)
        {
            var validationResult = await _validator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                return Result<ProductServiceModel>.Failure(Error.InvalidInput, validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            var data = await _business.CreateProductAsync(product);
            return Result<ProductServiceModel>.Success(data);
        }

        public async Task<Result<ProductServiceModel>> DeleteProductAsync(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                return Result<ProductServiceModel>.Failure(Error.InvalidInput, new List<string>() { "Product ID cannot be empty." });
            }
            var data = await _business.DeleteProductAsync(productId);
            return Result<ProductServiceModel>.Success(data);
        }

        public async Task<Result<ProductServiceModel>> FetchProductByIdAsync(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                return Result<ProductServiceModel>.Failure(Error.InvalidInput, new List<string>() { "Product ID cannot be empty." });
            }
            return Result<ProductServiceModel>.Success(await _business.FetchProductByIdAsync(productId));
        }

        public async Task<Result<ProductServiceModel>> UpdateProductAsync(ProductViewModel product)
        {
            var validationResult = await _validator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                return Result<ProductServiceModel>.Failure(Error.InvalidInput, validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            var data = await _business.UpdateProductAsync(product);
            return Result<ProductServiceModel>.Success(data);
        }
    }
}

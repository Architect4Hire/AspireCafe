using AspireCafe.AuthenticationDomainLayer.Business;
using AspireCafe.AuthenticationDomainLayer.Managers.Models.Service;
using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Service.OrderUpdate;
using AspireCafe.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.AuthenticationDomainLayer.Facade
{
    public class Facade : IFacade
    {
        private readonly IBusiness _business;

        public Facade(IBusiness business)
        {
            _business = business;
        }

        public async Task<Result<AuthenticationServiceModel>> GenerateTokenAsync(string username, string password)
        {
            var result = await _business.GenerateTokenAsync(username, password);
            if (result == null)
            {
                return Result<AuthenticationServiceModel>.Failure(Error.Unauthorized, new List<string>() { "" });
            }
            return Result<AuthenticationServiceModel>.Success(result);
        }

        public async Task<Result<AuthenticationServiceModel>> ValidateTokenAsync(string token)
        {
            var result = await _business.ValidateTokenAsync(token);
            if (result == null)
            {
                return Result<AuthenticationServiceModel>.Failure(Error.Forbidden, new List<string>() { "" });
            }
            return Result<AuthenticationServiceModel>.Success(result);
        }
    }
}

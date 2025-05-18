using AspireCafe.AuthenticationDomainLayer.Managers.Models.Service;
using AspireCafe.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.AuthenticationDomainLayer.Facade
{
    public interface IFacade
    {
        Task<Result<AuthenticationServiceModel>> GenerateTokenAsync(string username, string password);
        Task<Result<AuthenticationServiceModel>> ValidateTokenAsync(string token);
    }
}

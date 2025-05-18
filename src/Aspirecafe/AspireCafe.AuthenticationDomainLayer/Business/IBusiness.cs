using AspireCafe.AuthenticationDomainLayer.Managers.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.AuthenticationDomainLayer.Business
{
    public interface IBusiness
    {
        Task<AuthenticationServiceModel> GenerateTokenAsync(string username, string password);
        Task<AuthenticationServiceModel> ValidateTokenAsync(string token);
    }
}

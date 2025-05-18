using AspireCafe.Shared.Models.Service;

namespace AspireCafe.AuthenticationDomainLayer.Managers.Models.Service
{
    public class AuthenticationServiceModel:ServiceBaseModel
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}

using AspireCafe.Shared.Models.View;

namespace AspireCafe.AuthenticationDomainLayer.Managers.Models.View
{
    public class TokenRequestViewModel:ViewModelBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

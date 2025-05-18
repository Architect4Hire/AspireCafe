using AspireCafe.AuthenticationDomainLayer.Facade;
using AspireCafe.AuthenticationDomainLayer.Managers.Models.Service;
using AspireCafe.Shared.Results;
using AspireCafe.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using AspireCafe.AuthenticationDomainLayer.Managers.Models.View;

namespace AspireCafe.Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IFacade _facade;

        public AuthenticationController(IFacade facade)
        {
            _facade = facade;
        }

        [HttpPost("generate")]
        public async Task<Result<AuthenticationServiceModel>> GenerateToken(TokenRequestViewModel model)
        {
            var result = await _facade.GenerateTokenAsync(model.UserName,model.Password);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<AuthenticationServiceModel>.Failure(error, result.Messages)
            );
        }

        [HttpPost("validate")]
        public async Task<Result<AuthenticationServiceModel>> ValidateToken(TokenValidationViewModel model)
        {
            var result = await _facade.ValidateTokenAsync(model.Token);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<AuthenticationServiceModel>.Failure(error, result.Messages)
            );
        }
    }
}

using AspireCafe.AuthenticationDomainLayer.Facade;
using AspireCafe.AuthenticationDomainLayer.Managers.Models.Service;
using AspireCafe.Shared.Results;
using AspireCafe.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using AspireCafe.AuthenticationDomainLayer.Managers.Models.View;
using Microsoft.AspNetCore.Authorization;

namespace AspireCafe.Authentication.Controllers
{
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IFacade _facade;

        public AuthenticationController(IFacade facade)
        {
            _facade = facade;
        }

        /// <summary>
        /// Generates a new authentication token for a user based on provided credentials.
        /// </summary>
        /// <param name="model">The token request containing the username and password.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing a <see cref="Result{AuthenticationServiceModel}"/>.
        /// </returns>
        /// <response code="200">Token generated successfully and returned in the response.</response>
        /// <response code="400">Invalid input provided (e.g., missing or malformed username/password).</response>
        /// <response code="401">Authentication failed due to invalid credentials.</response>
        /// <response code="403">User is forbidden from generating a token.</response>
        /// <response code="500">Internal server error occurred while generating the token.</response>
        [HttpPost("generate")]
        [ProducesResponseType(typeof(Result<AuthenticationServiceModel>), 200)]
        [ProducesResponseType(typeof(Result<AuthenticationServiceModel>), 400)]
        [ProducesResponseType(typeof(Result<AuthenticationServiceModel>), 401)]
        [ProducesResponseType(typeof(Result<AuthenticationServiceModel>), 403)]
        [ProducesResponseType(typeof(Result<AuthenticationServiceModel>), 500)]
        public async Task<IActionResult> GenerateToken(TokenRequestViewModel model)
        {
            var result = await _facade.GenerateTokenAsync(model.UserName, model.Password);
            return result.Match();
        }

        /// <summary>
        /// Validates an existing authentication token.
        /// </summary>
        /// <param name="model">The token validation request containing the token to validate.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing a <see cref="Result{AuthenticationServiceModel}"/>.
        /// </returns>
        /// <response code="200">Token is valid and validation details are returned.</response>
        /// <response code="400">Invalid input provided (e.g., missing or malformed token).</response>
        /// <response code="401">Token is invalid or expired.</response>
        /// <response code="403">User is forbidden from validating the token.</response>
        /// <response code="500">Internal server error occurred while validating the token.</response>
        [HttpPost("validate")]
        [ProducesResponseType(typeof(Result<AuthenticationServiceModel>), 200)]
        [ProducesResponseType(typeof(Result<AuthenticationServiceModel>), 400)]
        [ProducesResponseType(typeof(Result<AuthenticationServiceModel>), 401)]
        [ProducesResponseType(typeof(Result<AuthenticationServiceModel>), 403)]
        [ProducesResponseType(typeof(Result<AuthenticationServiceModel>), 500)]
        public async Task<IActionResult> ValidateToken(TokenValidationViewModel model)
        {
            var result = await _facade.ValidateTokenAsync(model.Token);
            return result.Match();
        }
    }
}
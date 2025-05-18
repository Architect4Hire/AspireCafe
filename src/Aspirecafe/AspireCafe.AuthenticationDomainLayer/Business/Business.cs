using AspireCafe.AuthenticationDomainLayer.Managers.Models.Service;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AspireCafe.AuthenticationDomainLayer.Business
{
    public class Business:IBusiness
    {
        public async Task<AuthenticationServiceModel> GenerateTokenAsync(string username, string password)
        {
            // TODO: Implement real authentication logic
            if (username == "admin" && password == "password")
            {
                var key = Encoding.UTF8.GetBytes("your-256-bit-secret"); // TODO: Use secure config
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, username)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return new AuthenticationServiceModel { UserName = username, Token = tokenString };
            }
            return null;
        }

        public async Task<AuthenticationServiceModel> ValidateTokenAsync(string token)
        {
            // JWT validation logic
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("your-256-bit-secret"); // TODO: Use secure config
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, // Set to true and specify valid issuer in production
                ValidateAudience = false, // Set to true and specify valid audience in production
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var username = principal.Identity?.Name ?? principal.FindFirst(ClaimTypes.Name)?.Value;
                return new AuthenticationServiceModel { UserName = username };
            }
            catch
            {
                return null;
            }
        }
    }
}

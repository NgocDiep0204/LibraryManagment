using api_librarymanagment.Data.DTOs;
using api_librarymanagment.Models.Library;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace api_librarymanagment.Services
{
    public class TokenHandle : ITokenHandle
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        public TokenHandle(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSiginKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMonths(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSiginKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
        public JwtSecurityToken GetRefreshToken()
        {
            var authSiginKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddDays(7), 
                signingCredentials: new SigningCredentials(authSiginKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwt = _configuration.GetSection("Jwt");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Key"))),
                ValidateLifetime = true, // Chúng ta muốn token đã hết hạn vẫn được xác thực để lấy claims
                ValidIssuer = jwt["Issuer"],
                ValidAudience = jwt["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public string CreateToken(bool populateExp) 
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "1"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (populateExp)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddMinutes(15)).ToUnixTimeSeconds().ToString()));
            }

            var authSiginKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                signingCredentials: new SigningCredentials(authSiginKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

      

        /*Task<TokenDTO> ITokenHandle.CreateToken(bool populateExp)
        {
            var refreshToken = GenerateRefreshToken();
            if (populateExp)
            {

            }
        }*/
    }
}


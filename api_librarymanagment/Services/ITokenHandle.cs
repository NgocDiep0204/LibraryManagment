using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api_librarymanagment.Data.DTOs;

namespace api_librarymanagment.Services
{
    public interface ITokenHandle
    {
        public JwtSecurityToken GetToken(List<Claim> authClaims);
        public JwtSecurityToken GetRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        public string GenerateRefreshToken();   

       // public Task<TokenDTO> CreateToken(bool populateExp);

    }
}

using api_librarymanagment.Data.DTOs;
using Microsoft.EntityFrameworkCore;
using api_librarymanagment.Data;
using api_librarymanagment.Models.Login;


namespace api_librarymanagment.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetRefreshTokenAsync(string userId)
        {
            var refreshToken = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.Revoked == null && rt.Expires >= DateTime.UtcNow)
                .OrderByDescending(rt => rt.Created)
                .Select(rt => rt.Token)
                .FirstOrDefaultAsync();

            return refreshToken;
        }

        public async Task SaveRefreshTokenAsync(string userId, string refreshToken)
        {
            var newStoredRefreshToken = new RefreshToken
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(1), // Example: Set expiration for 1 month
                Created = DateTime.UtcNow,
               // CreatedByIp = "127.0.0.1", // Example: Get the user's IP address from HttpContext if needed
                UserId = userId,
            };

            _context.RefreshTokens.Add(newStoredRefreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
        {
            var validToken = await _context.RefreshTokens
                .AnyAsync(rt => rt.UserId == userId && rt.Token == refreshToken && rt.Expires >= DateTime.UtcNow && rt.Revoked == null);

            return validToken;
        }
    }
}

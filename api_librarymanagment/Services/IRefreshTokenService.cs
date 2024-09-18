namespace api_librarymanagment.Services
{
    public interface IRefreshTokenService
    {
        Task<string> GetRefreshTokenAsync(string userId);
        Task SaveRefreshTokenAsync(string userId, string refreshToken);
        Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken);
    }
}

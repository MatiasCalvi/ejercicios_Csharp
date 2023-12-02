namespace Datos.Interfaces
{
    public interface IDaoBDAccesUser
    {
        Task<string> GetRefreshTokenAsync(int userId);
        Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken);
        Task DeleteRefreshTokenAsync(int userId);
        Task StoreRefreshTokenAsync(int userId, string refreshToken);
    }
}

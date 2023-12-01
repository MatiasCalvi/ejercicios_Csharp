namespace Datos.Interfaces
{
    public interface IDaoBDAccesUser
    {
        void StoreRefreshToken(int userId, string refreshToken);
        Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken);
        Task DeleteRefreshTokenAsync(int userId);
        Task StoreRefreshTokenAsync(int userId, string refreshToken);
    }
}

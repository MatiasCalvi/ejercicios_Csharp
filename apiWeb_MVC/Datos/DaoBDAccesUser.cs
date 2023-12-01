using Datos.Interfaces;
using System.Data;
using Dapper;
using Datos.Exceptions;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using Configuracion;

namespace Datos
{
    public class DaoBDAccesUser : IDaoBDAccesUser
    {
        private readonly string connectionString;
        private const string existingTokenQuery = "SELECT RefreshTU_Token FROM refreshtokenu WHERE RefreshTU_UserID = @UserId";
        private const string updateTokenQuery = "UPDATE refreshtokenu SET RefreshTU_Token = @RefreshToken WHERE RefreshTU_UserID = @UserId";
        private const string createTokenQuery = "INSERT INTO refreshtokenu (RefreshTU_UserID, RefreshTU_Token) VALUES (@UserId, @RefreshToken)";
        private const string validateTokenQuery = "SELECT 1 FROM refreshtokenu WHERE RefreshTU_UserID = @UserId AND RefreshTU_Token = @RefreshToken";
        private const string deleteTokenQuery = "DELETE FROM refreshtokenu WHERE RefreshTU_UserID = @UserId";
        public DaoBDAccesUser(IOptions<BDConfiguration> dbConfig)
        {
            connectionString = dbConfig.Value.ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public void StoreRefreshToken(int userId, string refreshToken)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                var existingToken = dbConnection.QueryFirstOrDefault<string>(existingTokenQuery, new { UserId = userId });

                if (existingToken != null)
                {
                    dbConnection.Execute(updateTokenQuery, new { UserId = userId, RefreshToken = refreshToken });
                }
                else
                {
                    dbConnection.Execute(createTokenQuery, new { UserId = userId, RefreshToken = refreshToken });
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException("Error storing the refresh token in the database.", ex);
            }
        }

        public async Task StoreRefreshTokenAsync(int userId, string refreshToken)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                var existingToken = await dbConnection.QueryFirstOrDefaultAsync<string>(existingTokenQuery, new { UserId = userId });

                if (existingToken != null)
                {
                    await dbConnection.ExecuteAsync(updateTokenQuery, new { UserId = userId, RefreshToken = refreshToken });
                }
                else
                {
                    await dbConnection.ExecuteAsync(createTokenQuery, new { UserId = userId, RefreshToken = refreshToken });
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException("Error storing the refresh token in the database.", ex);
            }
        }


        public async Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                var isValidToken = await dbConnection.QueryFirstOrDefaultAsync<bool>(validateTokenQuery, new { UserId = userId, RefreshToken = refreshToken });

                return isValidToken;
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Error validating the refresh token in the database.", ex);
            }
        }

        public async Task DeleteRefreshTokenAsync(int userId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                await dbConnection.ExecuteAsync(deleteTokenQuery, new { UserId = userId });
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error deleting refresh token for user with ID {userId}.", ex);
            }
        }
    }
}

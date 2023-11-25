using System.Data;
using Dapper;
using Datos.Exceptions;
using Datos.Interfaces;
using Datos.Schemas;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using Configuracion;

namespace Datos
{
    public class DaoBD : IDaoBD
    {
        private readonly string connectionString; 
        private const string getAllUserQuery = "SELECT * FROM users";
        private const string getUserByIDQuery = "SELECT * FROM users WHERE user_ID = @user_ID";
        private const string createUserQuery = "INSERT INTO users(User_Name, User_LastName, User_Email, User_Password, User_CreationDate) VALUES(@User_Name, @User_LastName, @User_Email, @User_Password, @User_CreationDate); SELECT* FROM users WHERE User_ID = LAST_INSERT_ID()";
        private const string deletedUserQuery = "DELETE FROM users WHERE User_ID = @User_ID";
        private const string disableUserQuery = "UPDATE users SET User_Status = 0 WHERE User_ID = @User_ID";
        private const string getUserByEmailQuery = "SELECT * FROM users WHERE user_Email = @user_Email;";

        public DaoBD(IOptions<BDConfiguration> dbConfig)
        {
            connectionString = dbConfig.Value.ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public List<UserOutput> GetAllUsers()
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return dbConnection.Query<UserOutput>(getAllUserQuery).ToList();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Failed to get all users.", ex);
            }
        }

        public UserOutput? GetUserByID(int pId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return dbConnection.Query<UserOutput>(getUserByIDQuery, new { user_ID = pId }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException($"Error getting user with ID {pId}.", ex);
            }
        }

        public UserInputUpdate? GetUserByIDU(int pId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                {
                    dbConnection.Open();
                    return dbConnection.Query<UserInputUpdate>(getUserByIDQuery, new { user_ID = pId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException($"Error getting user with ID {pId}.", ex);
            }
        }

        public UserInputUpdate GetUserByEmail(string pEmail)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return dbConnection.Query<UserInputUpdate>(getUserByEmailQuery, new { user_Email = pEmail }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException($"Error getting user by email {pEmail}.", ex);
            }
        }

        public UserOutputCreate CreateNewUser(UserInput pUserInput)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                {
                    dbConnection.Open();
                    return dbConnection.QuerySingle<UserOutputCreate>(createUserQuery, pUserInput);
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException("Error creating a new user.", ex);
            }
        }

        public bool UpdateUser(int pId, UserInputUpdate pCurrentUser)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                {
                    dbConnection.Open();

                    List<string> updateFields = new();
                    DynamicParameters parameters = new();

                    if (pCurrentUser.User_Name != null)
                    {
                        updateFields.Add("User_Name = @User_Name");
                        parameters.Add("User_Name", pCurrentUser.User_Name);
                    }

                    if (pCurrentUser.User_LastName != null)
                    {
                        updateFields.Add("User_LastName = @User_LastName");
                        parameters.Add("User_LastName", pCurrentUser.User_LastName);
                    }

                    if (pCurrentUser.User_Email != null)
                    {
                        updateFields.Add("User_Email = @User_Email");
                        parameters.Add("User_Email", pCurrentUser.User_Email);
                    }

                    if (pCurrentUser.User_Password != null)
                    {
                        updateFields.Add("User_Password = @User_Password");
                        parameters.Add("User_Password", pCurrentUser.User_Password);
                    }

                    if (updateFields.Count == 0) return false;

                    parameters.Add("User_ID", pId);
                    updateFields.Add("User_UpdateDate = @User_UpdateDate");
                    parameters.Add("User_UpdateDate", pCurrentUser.User_UpdateDate);

                    string updateUserQuery = $"UPDATE users SET {string.Join(", ", updateFields)} WHERE User_ID = @User_ID";

                    int rowsAffected = dbConnection.Execute(updateUserQuery, parameters);

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error updating user with ID {pId}.", ex);
            }
        }

        public bool DisableUser(int pUserId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                {
                    dbConnection.Open();
                    int rowsAffected = dbConnection.Execute(disableUserQuery, new { User_ID = pUserId });

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error disabling user with ID {pUserId}.", ex);
            }
        }

        public void DeletedUser(int pId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                dbConnection.Execute(deletedUserQuery, new { User_ID = pId });
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error deleting user with ID {pId}.", ex);
            }
        }
    }
}
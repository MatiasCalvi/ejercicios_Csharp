using System.Data;
using Dapper;
using Datos.Exceptions;
using Datos.Interfaces;
using Datos.Schemas;
using MySql.Data.MySqlClient;

namespace Datos
{
    public class DaoBD : IDaoBD
    {
        private const string connectionString = "Server=localhost;Database=apiweb_mvc;Uid=root;Pwd=12345678";
        private const string getAllUserQuery = "SELECT * FROM users";
        private const string getUserByIDQuery = "SELECT * FROM users WHERE user_ID = @user_ID";
        private const string createUserQuery = "INSERT INTO users(User_Name, User_LastName, User_Email, User_Password, User_CreationDate) VALUES(@User_Name, @User_LastName, @User_Email, @User_Password, @User_CreationDate); SELECT* FROM users WHERE User_ID = LAST_INSERT_ID()";
        private const string deletedUserQuery = "DELETE FROM users WHERE User_ID = @User_ID";
        private const string disableUserQuery = "UPDATE users SET User_Status = 0 WHERE User_ID = @User_ID";
        public static IDbConnection Connection 
        {
            get
            {
                return new MySqlConnection(connectionString);
            }
        }

        public List<UserOutput> GetAllUsers()
        {
            try
            {
                using IDbConnection dbConnection = Connection;
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
                using IDbConnection dbConnection = Connection;
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
                using (IDbConnection dbConnection = Connection)
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

        public UserOutputCreate CreateNewUser(UserInput pUserInput)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
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
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();

                    List<string> updateFields = new List<string>();
                    DynamicParameters parameters = new DynamicParameters();

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
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    int rowsAffected = dbConnection.Execute(disableUserQuery, new { User_ID = pUserId });

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al deshabilitar el usuario con ID {pUserId}.", ex);
            }
        }

        public void DeletedUser(int pId)
        {
            try
            {
                using IDbConnection dbConnection = Connection;
                dbConnection.Open();
                dbConnection.Execute(deletedUserQuery, new { User_ID = pId });
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al eliminar el usuario con ID {pId}.", ex);
            }
        }
    }
}
﻿using System.Data;
using System.Text;
using Dapper;
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
        private const string createUserQuery = "INSERT INTO users(User_Name, User_LastName, User_Email, User_Password) VALUES(@User_Name, @User_LastName, @User_Email, @User_Password); SELECT* FROM users WHERE User_ID = LAST_INSERT_ID()";
        private const string deletedUserQuery = "DELETE FROM users WHERE User_ID = @User_ID";
        public static IDbConnection Connection 
        {
            get
            {
                return new MySqlConnection(connectionString);
            }
        }

        public List<UserOutput>GetAllUsers()
        {
            using IDbConnection dbConnection = Connection;
            dbConnection.Open();
            return dbConnection.Query<UserOutput>(getAllUserQuery).ToList();
        }

        public UserOutput? GetUserByID(int pId)
        {
            using IDbConnection dbConnection = Connection;
            dbConnection.Open();
            return dbConnection.Query<UserOutput>(getUserByIDQuery,new { user_ID = pId }).FirstOrDefault();
        }

        public UserUpdate? GetUserByIDU(int pId)
        {
            using IDbConnection dbConnection = Connection;
            dbConnection.Open();
            return dbConnection.Query<UserUpdate>(getUserByIDQuery, new { user_ID = pId }).FirstOrDefault();
        }

        public UserOutput CreateNewUser(UserInput pUserInput)
        {
            using IDbConnection dbConnection = Connection;
            dbConnection.Open();

            UserOutput userOutput = dbConnection.QuerySingle<UserOutput>(createUserQuery, pUserInput); 

            return userOutput; 
        }

        public bool UpdateUser(int id, UserUpdate pCurrentUser)
        {
            using IDbConnection dbConnection = Connection;
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

            parameters.Add("User_ID", id);

            string updateUserQuery = $"UPDATE users SET {String.Join(", ", updateFields)} WHERE User_ID = @User_ID";

            int rowsAffected = dbConnection.Execute(updateUserQuery, parameters);

            return rowsAffected > 0;
        }

        public void DeletedUser(int pId)
        {
            using IDbConnection dbConnection = Connection;
            dbConnection.Open();
            dbConnection.Execute(deletedUserQuery, new { User_ID = pId });
        }
    }
}
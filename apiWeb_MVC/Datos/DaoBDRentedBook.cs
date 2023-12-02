﻿using Configuracion;
using Dapper;
using Datos.Exceptions;
using Datos.Interfaces;
using Datos.Schemas;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Datos
{
    public class DaoBDRentedBook : IDaoBDRentedBook
    {
        private readonly string connectionString;
        private const string getAllRentendQuery = "SELECT * FROM rented_books";
        private const string getRentedByIDQuery = "SELECT * FROM rented_books WHERE RB_Id = @RB_Id";
        private const string createRentQuery = "INSERT INTO rented_books(RB_BookID, RB_UserID, RB_RentalStartDate,RB_RentDueDate,RB_Price) VALUES(@RB_BookID, @RB_UserID, @RB_RentalStartDate, @RB_RentDueDate, @RB_Price); SELECT* FROM rented_books WHERE RB_Id = LAST_INSERT_ID()";
        public DaoBDRentedBook(IOptions<BDConfiguration> dbConfig)
        {
            connectionString = dbConfig.Value.ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public async Task<List<RentedBookOut>> GetAllRentedAsync()
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return (await dbConnection.QueryAsync<RentedBookOut>(getAllRentendQuery)).ToList();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Failed to get the List of rented.", ex);
            }
        }

        public async Task<RentedBookOut?> GetRentByIDAsync(int pId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<RentedBookOut>(getRentedByIDQuery, new { RB_Id = pId });
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException($"Error getting rent with ID {pId}.", ex);
            }
        }

        public async Task<RentedBookOut> CreateNewRentAsync(RentedBook pBookInput)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return await dbConnection.QuerySingleAsync<RentedBookOut>(createRentQuery, pBookInput);
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException("Error creating a new rent.", ex);
            }
        }

    }
}

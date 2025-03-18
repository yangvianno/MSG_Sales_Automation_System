using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class MySqlDataAccess : IMySqlDataAccess
    {
        private readonly IConfiguration config;

        public string ConnectionStringName { get; set; } = "MySqlConnection";
        public string ConectionStr { get; set; } = "";
        public MySqlDataAccess(IConfiguration config)
        {
            this.config = config;
        }
        public MySqlDataAccess(string ConectionStr)
        {
            this.ConectionStr = ConectionStr;
        }
        public async Task<List<T>> loadData<T, U>(string s, U Parameter)
        {
            string connectionStr = config.GetConnectionString(ConnectionStringName);
            
            using (IDbConnection connection = new MySqlConnection(connectionStr))
            {
                var data = await connection.QueryAsync<T>(s, Parameter);
                return data.ToList();
            }
        }
        public Task getData(string s)
        {
            if (ConectionStr == "")
            {
                ConectionStr = config.GetConnectionString(ConnectionStringName);
            }
            using (MySqlConnection conn = new MySqlConnection(ConectionStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(s, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                    }
                }
            }

            return Task.CompletedTask;
        }

        public async Task Save<T>(string s, T Parameter)
        {
            if (ConectionStr == "")
            {
                ConectionStr = config.GetConnectionString(ConnectionStringName);
            }
            using (IDbConnection connection = new MySqlConnection(ConectionStr))
            {
                var data = await connection.ExecuteAsync(s, Parameter);
            }
        }

        public async Task execNonQuery(string s)
        {
            if (ConectionStr == "")
            {
                ConectionStr = config.GetConnectionString(ConnectionStringName);
            }
            using (IDbConnection connection = new MySqlConnection(ConectionStr))
            {
                var data = await connection.ExecuteAsync(s);
            }
        }
    }
}

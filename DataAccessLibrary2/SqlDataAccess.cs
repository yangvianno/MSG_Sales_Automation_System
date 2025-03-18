using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration config;

        public string ConnectionStringName { get; set; } = "DefaultConnection";
        public string ConectionStr { get; set; } = "";
        public SqlDataAccess(IConfiguration config)
        {
            this.config = config;
        }
        public SqlDataAccess(string ConectionStr)
        {
            this.ConectionStr = ConectionStr;
        }
        public async Task<List<T>> loadData<T, U>(string s, U Parameter, string cnstr = "")
        {
            if (ConectionStr == "")
            {
                ConectionStr = config.GetConnectionString(ConnectionStringName);
            }            
            if (cnstr=="")
            {
                cnstr = ConectionStr;
            }
            using (IDbConnection connection = new SqlConnection(cnstr))
            { 
                var data = await connection.QueryAsync<T>(s, Parameter, commandTimeout: 600);
                return data.ToList();
            }
        }
        public async Task Save<T>(string s, T Parameter, string cnstr = "")
        {
            if (ConectionStr == "")
            {
                ConectionStr = config.GetConnectionString(ConnectionStringName);
            }
            if (cnstr == "")
            {
                cnstr = ConectionStr;
            }
            using (IDbConnection connection = new SqlConnection(cnstr))
            {
                var data = await connection.ExecuteAsync(s, Parameter);
            }
        }

        public async Task execNonQuery(string s, string cnstr = "")
        {
            if (ConectionStr == "")
            {
                ConectionStr = config.GetConnectionString(ConnectionStringName);
            }
            if (cnstr == "")
            {
                cnstr = ConectionStr;
            }
            using (IDbConnection connection = new SqlConnection(cnstr))
            {
                var data = await connection.ExecuteAsync(s);
            }
        }

        public async Task<IEnumerable<dynamic>> getData(string s, string cnstr = "")
        {
            if (ConectionStr == "")
            {
                ConectionStr = config.GetConnectionString(ConnectionStringName);
            }
            if (cnstr == "")
            {
                cnstr = ConectionStr;
            }
            using (IDbConnection connection = new SqlConnection(cnstr))
            {
                return await connection.QueryAsync<dynamic>(s);
            }
        }

        public async Task<IEnumerable<IDictionary<string, object>>> getDataDic(string s, string cnstr = "")
        {
            if (ConectionStr == "")
            {
                ConectionStr = config.GetConnectionString(ConnectionStringName);
            }
            if (cnstr == "")
            {
                cnstr = ConectionStr;
            }
            using (IDbConnection connection = new SqlConnection(cnstr))
            {
                var result = await connection.QueryAsync<dynamic>(s);
                return result.Cast<IDictionary<string, object>>();
            }
        }
        public async Task<DataTable> getDataTable(string s, string cnstr = "")
        {
            if (ConectionStr == "")
            {
                ConectionStr = config.GetConnectionString(ConnectionStringName);
            }
            if (cnstr == "")
            {
                cnstr = ConectionStr;
            }
            using (var connection = new SqlConnection(cnstr))
            {
                DataTable table;
                try
                {
                    var cmd = new SqlCommand(s, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);                    
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    connection.Close();
                    cmd.Dispose();
                    table = dataSet.Tables[0];
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
                return table;
            }
        }
    }
}

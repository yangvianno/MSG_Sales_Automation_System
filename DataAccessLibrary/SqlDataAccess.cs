using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class SqlDataAccess
    {
        private readonly IConfiguration config;

        public string ConnectionStringName { get; set; } = "Default";
        public SqlDataAccess(IConfiguration config)
        {
            this.config = config;
        }
        public async Task<List<T>> loadData<T,U>(string s, U Parameter)
        {
            string connectionStr = config.GetConnectionString(ConnectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionStr))
            {

            }
        }
    }
}

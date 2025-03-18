using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IMySqlDataAccess
    {
        string ConnectionStringName { get; set; }

        Task execNonQuery(string s);
        Task getData(string s);
        Task<List<T>> loadData<T, U>(string s, U Parameter);
        Task Save<T>(string s, T Parameter);
    }
}
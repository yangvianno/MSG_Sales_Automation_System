using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface ISqlDataAccess
    {
        string ConnectionStringName { get; set; }

        Task<List<T>> loadData<T, U>(string s, U Parameter, string cnstr="");
        Task Save<T>(string s, T Parameter, string cnstr="");
        Task execNonQuery(string s, string cnstr="");
        Task<IEnumerable<dynamic>> getData(string s, string cnstr="");
        Task<DataTable> getDataTable(string s, string cnstr="");
        Task<IEnumerable<IDictionary<string, object>>> getDataDic(string s, string cnstr="");
    }
}
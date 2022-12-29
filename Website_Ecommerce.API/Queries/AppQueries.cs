using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Queries
{
    public class AppQueries : IAppQueries
    {
        public string _connectionString { get; }
        public AppQueries(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<ProductQueryModel>> GetListProductByShopId()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" select * from Products ");
                sb.Append(@" select * from Categories ");
                sb.Append(@" select * from Categories ");
                sb.Append(@" select * from Categories ");
                var result = await connection.QueryAsync<ProductQueryModel>(sb.ToString());
                return result.ToList();
            }
        }
    }
}
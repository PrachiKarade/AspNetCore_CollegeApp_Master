using System.Data;
using CoreWebApiSuperHero.Settings;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace CoreWebApiSuperHero.Data
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        private readonly SqlConnectionsSettings _options;
        public DbConnectionProvider(IOptions<SqlConnectionsSettings> options)
        {            
           _options = options.Value;
        }

        public IDbConnection CreateDBConnection()
        {
            var dbConnection = new SqlConnection(_options.CollegeAppDBConnection);
            dbConnection.Open();

            return dbConnection;
        }
    }
}

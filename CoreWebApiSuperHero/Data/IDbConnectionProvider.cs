using System.Data;
using Microsoft.Data.SqlClient;
namespace CoreWebApiSuperHero.Data
{
    public interface IDbConnectionProvider
    {
        IDbConnection CreateDBConnection();
    }
}

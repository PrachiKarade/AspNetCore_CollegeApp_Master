using System.Data;
using CoreWebApiSuperHero.Data.EntityModel;
using Dapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CoreWebApiSuperHero.Data.Repository
{
    public class CompanyRepository :ICompanyRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public CompanyRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            var SelectQuery = "SELECT * FROM Companies";

            using(var connection = _dbConnectionProvider.CreateDBConnection())
            {
                var companies = await connection.QueryAsync<Company>(SelectQuery);
                return companies.ToList();
            }
        }

        public async Task<Company> GetCompany(int id)
        {
            var SelectQuery = "SELECT * FROM Companies where Id = @Id" ;

            using (var connection = _dbConnectionProvider.CreateDBConnection())
            {
                var company = await connection.QuerySingleOrDefaultAsync<Company>(SelectQuery, new { Id = id });

                return company;
            }
        }

        public async Task<Company> CreateCompany(Company company)
        {
           var insertQuery = "INSERT INTO Companies (Name, Address, Country) VALUES (@Name, @Address, @Country);" +
                             "SELECT CAST(SCOPE_IDENTITY() as int);";

           /* var parameters = new DynamicParameters();
            parameters.Add("Name123", company.Name, DbType.String);
            parameters.Add("Address", company.Address,DbType.String);
            parameters.Add("Country", company.Country,DbType.String);*/

            using (var connection = _dbConnectionProvider.CreateDBConnection())
            {
                var newCompanyId = await connection.ExecuteScalarAsync<int>(insertQuery, company);
                company.Id = newCompanyId;
                return company;
            }
        }

        public async Task DeleteCompany(int id)
        {
            var deleteQuery = "DELETE FROM Companies WHERE Id = @id";

            using (var connection = _dbConnectionProvider.CreateDBConnection())
            {
                await connection.ExecuteAsync(deleteQuery, new { id });
            }
        }

        public async Task UpdateCompany(int id, Company company)
        {
            var updateQuery = "UPDATE Companies SET Name = @Name, Address = @Address, Country = @Country WHERE Id = @id";

            using (var connection = _dbConnectionProvider.CreateDBConnection())
            {
               await connection.ExecuteAsync(updateQuery, company);  
            }
        }

        public async Task<Company> GetCompanyByEmployeeId(int employeeId)
        {
            var sp_name = "sp_CompanyByEmployeeId";
            var parameters = new DynamicParameters();
            parameters.Add("Id", employeeId, DbType.Int32, ParameterDirection.Input);

            using (var connection = _dbConnectionProvider.CreateDBConnection())
            {
                var company = await connection.QueryFirstOrDefaultAsync<Company>(sp_name, parameters, commandType: CommandType.StoredProcedure);

                return company;
            }
        }

        public async Task<Company> GetCompanyEmployeeMultipleResults(int CompanyId)
        {
            var query = "SELECT * FROM Companies WHERE Id = @Id; " +
                        "SELECT * FROM Employee WHERE CompanyId = @Id";

            using (var connection = _dbConnectionProvider.CreateDBConnection())
            { 
                using (var multi = await connection.QueryMultipleAsync(query, new { Id = CompanyId }))
                {
                    var company = await multi.ReadSingleOrDefaultAsync<Company>();
                    if (company != null)
                    {
                        company.Employees = (await multi.ReadAsync<Employee>()).ToList();
                    }
                    return company;
                }
            }

        }

        public async Task<List<Company>> GetCompanyEmployeeMultipleMappings()
        {
            var query = "SELECT * FROM Companies c JOIN Employee e ON c.Id = e.CompanyId order by c.Id";

            using (var connection = _dbConnectionProvider.CreateDBConnection())
            {
                var companyDict = new Dictionary<int, Company>();
                var companies = await connection.QueryAsync<Company, Employee, Company>
                (
                    query,
                    (company, employee) =>
                    {
                        if (!companyDict.TryGetValue(company.Id, out var currentCompany))
                        {
                            currentCompany = company;
                            companyDict.Add(currentCompany.Id, currentCompany);
                        }
                        if (employee != null)
                        {
                            currentCompany.Employees.Add(employee);
                        }
                        return currentCompany;
                    },
                    splitOn: "Id"
                );
                return companyDict.Values.ToList();
            }
        }
    }
}

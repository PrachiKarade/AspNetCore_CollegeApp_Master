using CoreWebApiSuperHero.Data.EntityModel;

namespace CoreWebApiSuperHero.Data.Repository
{
    public interface ICompanyRepository
    {
        public  Task<IEnumerable<Company>> GetCompanies();

        public  Task<Company> GetCompany(int id);

        public  Task<Company> CreateCompany(Company company);

        public  Task UpdateCompany(int id, Company company);

        public  Task DeleteCompany(int id);

        public Task<Company> GetCompanyByEmployeeId(int employeeId);

        public Task<Company> GetCompanyEmployeeMultipleResults(int id);

        public Task<List<Company>> GetCompanyEmployeeMultipleMappings();
    }
}

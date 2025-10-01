using CoreWebApiSuperHero.Data.EntityModel;
using CoreWebApiSuperHero.Data.Repository;

namespace CoreWebApiSuperHero.Services
{
    public interface ICompanyService 
    {
        public Task<IEnumerable<CompanyDTO>> GetCompanies();

        public Task<CompanyDTO> GetCompany(int id);

        public Task<CompanyDTO> CreateCompany(CompanyDTO company);

        public Task UpdateCompany(int id, CompanyDTO company);

        public Task DeleteCompany(int id);

        public Task<CompanyDTO> GetCompanyByEmployeeId(int employeeId);

        public Task<CompanyDTO> GetCompanyEmployeeMultipleResults(int CompanyId);

        public Task<List<CompanyDTO>> GetCompanyEmployeeMultipleMappings();
    }
}

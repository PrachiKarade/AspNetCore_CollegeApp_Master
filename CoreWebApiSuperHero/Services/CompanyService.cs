using CoreWebApiSuperHero.Data.EntityModel;
using CoreWebApiSuperHero.Data.Repository;
using CoreWebApiSuperHero.Models;

namespace CoreWebApiSuperHero.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDTO>> GetCompanies()
        {
            var Companies = await _companyRepository.GetCompanies();

            return _mapper.Map<IEnumerable<CompanyDTO>>(Companies);

        }

        public async Task<CompanyDTO> GetCompany(int id)
        {
            var company = await _companyRepository.GetCompany(id);
            var response = _mapper.Map<CompanyDTO>(company);
            return response;
        }
        public async Task<CompanyDTO> CreateCompany(CompanyDTO companydto)
        {
            var companyEntity = _mapper.Map<Company>(companydto);

            var newCompany = await _companyRepository.CreateCompany(companyEntity);

            var response = _mapper.Map<CompanyDTO>(newCompany);           

            return response;
        }

        public async Task UpdateCompany(int id, CompanyDTO companydto)
        {
            var existingCompany = await _companyRepository.GetCompany(id);

            if(existingCompany == null)
            {
                throw new Exception($"Company with id {id} not found.");
            }
            var companyEntity = _mapper.Map<Company>(companydto);

            await _companyRepository.UpdateCompany(id, companyEntity);
        }

        public async Task DeleteCompany(int id)
        {
           var existingCompany = await _companyRepository.GetCompany(id);
            if (existingCompany == null)
            {
                throw new Exception($"Company with id {id} not found.");
            }
            await _companyRepository.DeleteCompany(id);
        }

        public async Task<CompanyDTO> GetCompanyByEmployeeId(int employeeId)
        {
            var company = await _companyRepository.GetCompanyByEmployeeId(employeeId);

            var response = _mapper.Map<CompanyDTO>(company);

            return response;
        }

        public async Task<CompanyDTO> GetCompanyEmployeeMultipleResults(int CompanyId)
        {
            var company = await _companyRepository.GetCompanyEmployeeMultipleResults(CompanyId);

            var response = _mapper.Map<CompanyDTO>(company);

            return response;
        }
        public async Task<List<CompanyDTO>> GetCompanyEmployeeMultipleMappings()
        { 
            var companies = await _companyRepository.GetCompanyEmployeeMultipleMappings();
            var response = _mapper.Map<List<CompanyDTO>>(companies);
            return response;
        }
    }
}

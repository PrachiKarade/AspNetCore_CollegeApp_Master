using CoreWebApiSuperHero.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApiSuperHero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private IMapper _mapper;
        public ApiResponse _apiresponse;
        private readonly ILogger<CompanyController> _logger;
        public CompanyController(ICompanyService companyService, IMapper mapper, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _mapper = mapper;
            _apiresponse = new ApiResponse();
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetCompanies()
        {
            try
            {
                var companies = await _companyService.GetCompanies();
                _apiresponse.Data = companies;
                _apiresponse.Status = true;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.OK;

                return Ok(_apiresponse);
            }
            catch (Exception ex)
            {
                _apiresponse.Status = false;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiresponse.Errors = new List<string>() { ex.ToString() };
                return _apiresponse;
            }
        }

        [HttpGet("{id:int}", Name = "GetCompany")]
        public async Task<ActionResult<ApiResponse>> GetCompany(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError($"Get Company Error with Id {id}");

                    _apiresponse.Errors.Add("Id must be greater than  zero ");
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
                var company = await _companyService.GetCompany(id);

                if (company == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(_apiresponse);
                }
                _apiresponse.Data = company;
                _apiresponse.Status = true;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_apiresponse);
            }
            catch (Exception ex)
            {
                _apiresponse.Status = false;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiresponse.Errors = new List<string>() { ex.ToString() };
                return _apiresponse;
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateCompany([FromBody] CompanyDTO companyDTO)
        {
            try
            {
                if (companyDTO == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_apiresponse);
                }
                var company = await _companyService.CreateCompany(companyDTO);

                _apiresponse.Data = company;
                _apiresponse.Status = true;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.Created;
                return CreatedAtRoute("GetCompany", new { id = company.Id }, _apiresponse);
            }
            catch (Exception ex)
            {
                _apiresponse.Status = false;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiresponse.Errors = new List<string>() { ex.ToString() };
                return _apiresponse;
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> UpdateCompany(int id, [FromBody] CompanyDTO companyDTO)
        {
            try
            {
                if (companyDTO == null || id != companyDTO.Id || id <= 0)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_apiresponse);
                }
                await _companyService.UpdateCompany(id, companyDTO);
                _apiresponse.Status = true;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.NoContent;
                return Ok(_apiresponse);
            }
            catch (Exception ex)
            {
                _apiresponse.Status = false;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiresponse.Errors = new List<string>() { ex.ToString() };
                return _apiresponse;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteCompany(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_apiresponse);
                }
                await _companyService.DeleteCompany(id);
                _apiresponse.Status = true;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.NoContent;
                return Ok(_apiresponse);
            }
            catch (Exception ex)
            {
                _apiresponse.Status = false;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiresponse.Errors = new List<string>() { ex.ToString() };
                return _apiresponse;
            }
        }

        [HttpGet("GetCompanyByEmployeeId/{employeeId:int}")]        
        public async Task<ActionResult<ApiResponse>> GetCompanyByEmployeeId(int employeeId)
        {
            try
            {
                if (employeeId <= 0)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_apiresponse);
                }
                var company = await _companyService.GetCompanyByEmployeeId(employeeId);
                if (company == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(_apiresponse);
                }
                _apiresponse.Data = company;
                _apiresponse.Status = true;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_apiresponse);
            }
            catch (Exception ex)
            {
                _apiresponse.Status = false;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiresponse.Errors = new List<string>() { ex.ToString() };
                return _apiresponse;
            }
        }

        [HttpGet("GetCompanyEmployeeMultipleResults/{companyId:int}")]
        public async Task<ActionResult<ApiResponse>> GetCompanyEmployeeMultipleResults(int companyId)
        {
            try
            {
                if (companyId <= 0)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_apiresponse);
                }
                var company = await _companyService.GetCompanyEmployeeMultipleResults(companyId);
                if (company == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(_apiresponse);
                }
                _apiresponse.Data = company;
                _apiresponse.Status = true;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_apiresponse);
            }
            catch (Exception ex)
            {
                _apiresponse.Status = false;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiresponse.Errors = new List<string>() { ex.ToString() };
                return _apiresponse;
            }
        }

        [HttpGet("GetCompanyEmployeeMultipleMappings")]
        public async Task<ActionResult<ApiResponse>> GetCompanyEmployeeMultipleMappings()
        {
            try
            {               
                var company = await _companyService.GetCompanyEmployeeMultipleMappings();

                if (company == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(_apiresponse);
                }
                _apiresponse.Data = company;
                _apiresponse.Status = true;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_apiresponse);
            }
            catch (Exception ex)
            {
                _apiresponse.Status = false;
                _apiresponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiresponse.Errors = new List<string>() { ex.ToString() };
                return _apiresponse;
            }
        }
    }
}

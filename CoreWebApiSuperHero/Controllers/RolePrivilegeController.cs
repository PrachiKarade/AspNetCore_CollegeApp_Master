using System.Net;
using CoreWebApiSuperHero.Data;
using CoreWebApiSuperHero.Data.Repository;
using CoreWebApiSuperHero.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApiSuperHero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePrivilegeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ApiResponse _apiResponse;
        private ICollegeRepository<RolePrivilege> _rolePrivilegeRepository;

        public RolePrivilegeController(IMapper mapper, ICollegeRepository<RolePrivilege> rolePrivilegeRepository)
        {
            _mapper = mapper;
            _apiResponse = new ApiResponse();
            _rolePrivilegeRepository = rolePrivilegeRepository;
        }

        [HttpGet]
        [Route("All", Name = "GetAllRolePrivilege")]
        [ProducesResponseType(StatusCodes.Status200OK)]         // successfully requested if the Role is found
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> GetAllRolePrivileges()
        {
            try
            {
                var allRolePrivilege = await _rolePrivilegeRepository.GetAllAsync();

                _apiResponse.Data = _mapper.Map<List<RolePrivilegeDTO>>(allRolePrivilege); 
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return _apiResponse;

            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.Errors = new List<string>() { ex.ToString() };
                return BadRequest(_apiResponse);
            }
        }


        [HttpGet]
        [Route("{RolePrivilegeId:int}", Name = "GetRolePrivilegeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]         //
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> GetRolePrivilegeById(int RolePrivilegeId)
        {
            try
            {
                if (RolePrivilegeId <= 0)
                    return BadRequest();

                var rolePrivilege = await _rolePrivilegeRepository.GetByFilterAsync(role => role.RolePrivilegeId == RolePrivilegeId, false);

                if (rolePrivilege == null)
                    return NotFound($" Role privilege not found with the RolePrivilegeId: {RolePrivilegeId}");

                _apiResponse.Data = _mapper.Map<RolePrivilegeDTO>(rolePrivilege);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return _apiResponse;
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _apiResponse.Errors = new List<string>() { ex.Message };
                return BadRequest(_apiResponse);
            }
        }

        [HttpGet]
        [Route("{RolePrivilegeName:alpha}", Name = "GetRolePrivilegeByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]         //requested if the Role is found
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> GetRolePrivilegeByName(string RolePrivilegeName)
        {
            try
            {
                if (String.IsNullOrEmpty(RolePrivilegeName))
                    return BadRequest();

                var rolePrivilege = await _rolePrivilegeRepository.GetByFilterAsync(role => role.RolePrivilegeName.Contains(RolePrivilegeName), false);

                if (rolePrivilege == null)
                    return NotFound($"Role Privilege not found with the role privilege name: {RolePrivilegeName}");

                _apiResponse.Data = _mapper.Map<RolePrivilegeDTO>(rolePrivilege);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return _apiResponse;
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _apiResponse.Errors = new List<string>() { ex.Message };
                return BadRequest(_apiResponse);
            }
        }


        [HttpGet]
        [Route("GetAllRolePrivilegeByRoleId", Name = "GetAllRolePrivilegesByRoleId")]
        [ProducesResponseType(StatusCodes.Status200OK)]         // successfully requested if the Role is found
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> GetAllRolePrivilegesByRoleId(int RoleId)
        {
            try
            {
                var allRolePrivilege = await _rolePrivilegeRepository.GetAllByFilterAsync(rolePrivilege => rolePrivilege.RoleId == RoleId,false);

                _apiResponse.Data = _mapper.Map<List<RolePrivilegeDTO>>(allRolePrivilege); 
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return _apiResponse;

            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.Errors = new List<string>() { ex.ToString() };
                return BadRequest(_apiResponse);
            }
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> CreateRolePrivilege([FromBody] RolePrivilegeDTO rolePrivilegeDTOObj)
        {
            try
            {
                if (rolePrivilegeDTOObj == null)
                    return BadRequest();

                if (rolePrivilegeDTOObj.RolePrivilegeName == null)
                    return NotFound();

                var rolePrivilegeObj = _mapper.Map<RolePrivilege>(rolePrivilegeDTOObj);

                rolePrivilegeObj.Createdate = DateTime.Now;
                rolePrivilegeObj.Updatedate = DateTime.Now;

                var createdRolePrivilege = await _rolePrivilegeRepository.CreateAsync(rolePrivilegeObj);
                rolePrivilegeDTOObj.RolePrivilegeId = createdRolePrivilege.RolePrivilegeId;

                _apiResponse.Status = true;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.Created;
                _apiResponse.Data = _mapper.Map<RolePrivilegeDTO>(createdRolePrivilege);
                //return Ok(_apiResponse);
                return CreatedAtRoute("GetRolePrivilegeById", new { id = rolePrivilegeDTOObj.RolePrivilegeId }, _apiResponse);

            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.Errors = new List<string> { ex.Message };

                return BadRequest(_apiResponse);
            }
        }

        [HttpPut]
        [Route("UpdateRolePrivilege")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> UpdateRolePrivilege([FromBody] RolePrivilegeDTO rolePrivilegeDTOObj)
        {
            try
            {
                if (rolePrivilegeDTOObj == null)
                    return BadRequest();


                var existingrolePrivilegeObj = await _rolePrivilegeRepository.GetByFilterAsync(role => role.RolePrivilegeId == rolePrivilegeDTOObj.RolePrivilegeId, true);

                if (existingrolePrivilegeObj == null)
                    return NotFound();

                var newRolePrivilegeObj = _mapper.Map<RolePrivilege>(rolePrivilegeDTOObj);

                newRolePrivilegeObj.Updatedate = DateTime.UtcNow;

                await _rolePrivilegeRepository.UpdateAsync(newRolePrivilegeObj);

                _apiResponse.Status = true;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                _apiResponse.Data = _mapper.Map<RolePrivilegeDTO>(newRolePrivilegeObj);
                //return Ok(_apiResponse);
                return CreatedAtRoute("GetRolePrivilegeById", new { id = rolePrivilegeDTOObj.RolePrivilegeId }, _apiResponse);

            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.Errors = new List<string> { ex.Message };

                return BadRequest(_apiResponse);
            }
        }

        [HttpDelete]
        [Route("{RolePrivilegeId:int}", Name ="DeleteRolePrivilege")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> DeleteRolePrivilege(int RolePrivilegeId)
        {
            try
            {
                if ( RolePrivilegeId <= 0)
                    return BadRequest();


                var existingrolePrivilegeObj = await _rolePrivilegeRepository.GetByFilterAsync(role => role.RolePrivilegeId == RolePrivilegeId, true);

                if (existingrolePrivilegeObj == null)
                    return NotFound();

                var result = await _rolePrivilegeRepository.DeleteAsync(existingrolePrivilegeObj);

                _apiResponse.Status = true;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                _apiResponse.Data = result;
                
                 return Ok(_apiResponse);                
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.Errors = new List<string> { ex.Message };

                return BadRequest(_apiResponse);
            }
        }

    }
}

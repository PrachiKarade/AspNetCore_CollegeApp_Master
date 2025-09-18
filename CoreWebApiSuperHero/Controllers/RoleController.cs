using CoreWebApiSuperHero.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreWebApiSuperHero.Data;
using System.Net;
using Microsoft.AspNetCore.Cors;

namespace CoreWebApiSuperHero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class RoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Role> _roleRepository;
        private ApiResponse _apiResponse;
        public RoleController(IMapper mapper, ICollegeRepository<Role> roleRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _apiResponse = new ApiResponse();//or new() this is used to initialize the ApiResponse object
        }

        [HttpGet]
        [Route("All", Name = "GetAllRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]         // successfully requested if the Role is found
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> GetAllRoles()
        {
            try
            {
                var allRoles = await _roleRepository.GetAllAsync();

                _apiResponse.Data = _mapper.Map<List<RoleDTO>>(allRoles); ;
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
        [Route("{RoleId:int}", Name = "GetRoleById")]
        [ProducesResponseType(StatusCodes.Status200OK)]         //
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> GetRoleById(int RoleId)
        {
            try
            {
                if (RoleId <= 0)
                    return BadRequest();

                var role = await _roleRepository.GetByFilterAsync(role => role.RoleId == RoleId, false);

                if (role == null)
                    return NotFound($"Role not found with the RoleId: {RoleId}");

                _apiResponse.Data = _mapper.Map<RoleDTO>(role);
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
        [Route("{RoleName:alpha}", Name = "GetRoleByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]         //requested if the Role is found
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> GetRoleByName(string RoleName)
        {
            try
            {
                if (String.IsNullOrEmpty(RoleName))
                    return BadRequest();

                var role = await _roleRepository.GetByFilterAsync(role => role.RoleName == RoleName, false);

                if (role == null)
                    return NotFound($"Role not found with the role name: {RoleName}");

                _apiResponse.Data = _mapper.Map<RoleDTO>(role);
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]         //if the Role is created successfully
        [ProducesResponseType(StatusCodes.Status201Created)]    //if the Role is created successfully
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> CreateRoleAsync([FromBody] RoleDTO objRoleDTO)
        {
            try
            {

                if (objRoleDTO == null)
                    return BadRequest();

                if (objRoleDTO.RoleName == null)
                    return NotFound();

                Role objRole = _mapper.Map<Role>(objRoleDTO);
                objRole.Createdate = DateTime.UtcNow;
                objRole.Updatedate = DateTime.UtcNow;
                objRole.IsDeleated = false;

                var Result = await _roleRepository.CreateAsync(objRole);
                objRoleDTO.RoleId = Result.RoleId;

                _apiResponse.Data = objRoleDTO;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);

                //return CreatedAtRoute("GetRoleById", new { id = objRoleDTO.RoleId }, _apiResponse); // this will return the created Role with a 201 Created status code

            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _apiResponse.Errors = new List<string>() { ex.Message };
                return BadRequest(_apiResponse);
            }
        }

        [HttpPut(Name = "UpdateRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]         //if the Role is updated successfully        
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> UpdateRole(RoleDTO objRoleDTO)
        {
            try
            {
                if (objRoleDTO == null)
                    return BadRequest();

                var existingRole = await _roleRepository.GetByFilterAsync(role => role.RoleId == objRoleDTO.RoleId, true);

                if (existingRole == null)
                    return NotFound($"Role not found with the RoleId: {objRoleDTO.RoleId}");                

                _mapper.Map(objRoleDTO, existingRole);//map the objRoleDTO to newRole object of type Role

                existingRole.Createdate = existingRole.Createdate; //keep the original created date
                existingRole.Updatedate = DateTime.UtcNow;

                await _roleRepository.UpdateAsync(existingRole);

                _apiResponse.Data = existingRole;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _apiResponse.Errors = new List<string>() { ex.Message };
                return BadRequest(_apiResponse);
            }
        }

        [HttpDelete]
        [Route("{RoleId:int}",Name = "DeleteRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]         //if the Role is deleted successfully
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> DeleteRole(int RoleId)
        {
            try
            {
                if (RoleId <= 0)
                    return BadRequest();

                var existingRole = await _roleRepository.GetByFilterAsync(role => role.RoleId == RoleId, false);                
                
                var result = await _roleRepository.DeleteAsync(existingRole);

                _apiResponse.Data = result;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _apiResponse.Errors = new List<string>() { ex.Message };
                return BadRequest(_apiResponse);
            }
        }
    }
}

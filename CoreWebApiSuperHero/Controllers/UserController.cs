using System.Net;
using CoreWebApiSuperHero.Data;
using CoreWebApiSuperHero.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CoreWebApiSuperHero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        
        ApiResponse _apiResponse;
        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger) 
        {
            this._userService = userService;
            this._apiResponse = new ApiResponse();
            this._mapper = mapper;
            this._logger = logger;
        }

        [HttpGet]
        [Route("All", Name = "GetAllUsers")] // rouute attribute is used to specify the route for this action method. Name is used to give a name to the route
        [ProducesResponseType(StatusCodes.Status200OK)]         //if the Role is updated successfully        
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Getting all users method started");

                var result = await _userService.GetAllUsersAsync();

                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = result;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.Errors.Add(ex.Message);
                return BadRequest(_apiResponse);
            }
        }


        [HttpGet]
        [Route("{UserId:int}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]         //
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> GetUserById(int UserId)
        {
            try
            {
                if (UserId <= 0)
                    return BadRequest();

                var user = await _userService.GetUserByIdAsync(UserId);

                if (user == null)
                {
                   _logger.LogError($"User not found with the UserId: {UserId}");
                    return NotFound($"Role not found with the RoleId: {UserId}");
                }

                _apiResponse.Data = user;
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
        [Route("CreateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]         //if the Role is updated successfully        
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> CreateUserAsync([FromBody] UserDTO userdto)
        {
            try
            {
                var result = await _userService.CreateUserBySPAsync(userdto); //await _userService.CreateUserAsync(userdto);

                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = result;
                
                return Ok(_apiResponse);

            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.Errors.Add(ex.Message);

                return BadRequest(_apiResponse);
            }
        }


        [HttpPut(Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]         //if the Role is updated successfully        
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult<ApiResponse>> UpdateUser(UserDTO objUserDTO)
        {
            try
            {
                if (objUserDTO == null)
                    return BadRequest();

                var result = await _userService.UpdateUserAsync(objUserDTO);

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


        [HttpDelete("{id:int}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]         //if the Role is updated successfully        
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if the RoleId is less than or equal to 0
        [ProducesResponseType(StatusCodes.Status404NotFound)]   //if the Role with the given RoleId is not found
        [ProducesResponseType(StatusCodes.Status403Forbidden)]  //if we have authorization
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//if there is any server error
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Delete User method called with id: {id}");
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid user ID provided.");
                    return BadRequest("user ID must be greater than zero.");
                }

                var result = await _userService.DeleteUserAsync(id);// Using the repository to get the existing student from the database

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

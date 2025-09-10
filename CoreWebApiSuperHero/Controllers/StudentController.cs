using CoreWebApiSuperHero.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using CoreWebApiSuperHero.Data.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Net;


namespace CoreWebApiSuperHero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [EnableCors("AllowAll")] // Enable CORS for this controller using the "MyOnlyLocalhost" policy defined in Program.cs
    //[Authorize(AuthenticationSchemes = "LocalUsers")] // This attribute is used to specify that the controller requires authentication
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        //private readonly CollegeDBContext _collegeDBContext;
        private readonly IMapper _mapper;

        // private readonly ICollegeRepository<Student> _studentRepository;
        private readonly IStudentRepository _studentRepository; // Using the specific repository interface for Student because it might have additional methods specific to Student entity.

        private ApiResponse _apiResponse;

        #region Constructor
        public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepository, IMapper mapper)
        {
            _logger = logger;            
            _mapper = mapper;
            _studentRepository = studentRepository;
            _apiResponse = new ApiResponse(); // or _apiResponse = new();
        }

        #endregion

        #region Get  Students

        [HttpGet(Name = "GetStudentsAsync")]
        public async Task<ActionResult<ApiResponse>> GetStudentsAsync()
        {
            try
            {
                _logger.LogInformation("GetStudents method called");
                // Fetching all students from the database and projecting to an anonymous type
                // This is done to avoid sending the entire entity with all properties, especially if there are sensitive data.

                //var students = await _collegeDBContext.Students.ToListAsync();// this will return all the students from the database

                var students = await _studentRepository.GetAllAsync();// this will return all the students from the database

                if (students == null || !students.Any())
                {
                    return NotFound("No students found.");
                }
                // Map the list of Student entities to a list of StudentDTOs
                //var studentDTOs = _mapper.Map<List<StudentDTO>>(students);// Using AutoMapper to map the list of Student entities to a list of StudentDTOs.Map<Destination>(Source)
                _apiResponse.Data = _mapper.Map<List<StudentDTO>>(students);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Status = true;

                return _apiResponse;
            }
            catch (Exception ex) 
            { 
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                return _apiResponse;
            }
        }

        [HttpGet("{id:int}", Name = "GetStudentById")]// This route will match requests like /api/student/1
        /*[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]*/
        public async Task<ActionResult> GetStudentByIdAsync(int id)
        {
            _logger.LogInformation($"GetStudentById method called with id: {id}");

            if (id <= 0)
            {
                _logger.LogWarning("Invalid student ID provided.");
                return BadRequest("Student ID must be greater than zero.");
            }
            //var student = await _collegeDBContext.Students.FirstOrDefaultAsync(s => s.Id == id);
            var student = await _studentRepository.GetByFilterAsync(student => student.Id == id, true);// Using the repository to get the student by ID with no tracking

            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            var studentDto = _mapper.Map<StudentDTO>(student);// Using AutoMapper to map the Student entity to a StudentDTO
                        
            return Ok(studentDto);
        }

        [HttpGet("{name}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetStudentByNameAsync(string studentName)
        {
            _logger.LogInformation($"GetStudentByName method called with name: {studentName}");

            if (string.IsNullOrEmpty(studentName))
            {
                _logger.LogWarning("Student name is null or empty.");
                return BadRequest("Student name cannot be null or empty.");
            }

            //var student = await _collegeDBContext.Students.Where(s => s.StudentName == studentName).ToListAsync();

            var student = await _studentRepository.GetByFilterAsync(student => student.StudentName.ToLower().Contains(studentName.ToLower()), true);
            // Using LINQ to find the student by name
            if (student == null)
            {
                _logger.LogWarning($"Student with name {studentName} not found.");
                return NotFound($"Student with name {studentName} not found.");
            }

            var studentDto = _mapper.Map<StudentDTO>(student);// Using AutoMapper to map the Student entity to a StudentDTO

            return Ok(studentDto);
        }

        #endregion

        #region Student Add, Update, Delete

        [HttpPost(Name = "AddStudentAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddStudentAsync([FromBody] StudentDTO dto)
        {
            _logger.LogInformation("AddStudent method called");
            if (dto == null)
            {
                _logger.LogWarning("Received null student object.");
                return BadRequest("Student object cannot be null.");
            }
            if (string.IsNullOrEmpty(dto.StudentName))
            {
                _logger.LogWarning("Student name is required.");
                return BadRequest("Student name is required.");
            }

            Student newStudent = _mapper.Map<Student>(dto);// Using AutoMapper to map the StudentDTO to a Student entity
           
            /*await _collegeDBContext.Students.AddAsync(newStudent);
            await _collegeDBContext.SaveChangesAsync();*/

            var studentAfterCreation = await _studentRepository.CreateAsync(newStudent);// Using the repository to add the new student to the database
            newStudent.Id = studentAfterCreation.Id; //  Id is auto-generated by the database, you might not need to set it here.

            return CreatedAtRoute(nameof(GetStudentByIdAsync), new { id = dto.Id }, dto);// This will return the created student with a 201 Created status code and the location of the new resource in the response header
        }

        [HttpPut("{id:int}", Name = "UpdateStudent")]
        public async Task<ActionResult> UpdateStudentAsync(int id, [FromBody] StudentDTO studentDTO)
        {
            _logger.LogInformation($"UpdateStudent method called with id: {id}");
            if (id <= 0)
            {
                _logger.LogWarning("Invalid student ID provided.");
                return BadRequest("Student ID must be greater than zero.");
            }
            if (studentDTO == null)
            {
                _logger.LogWarning("Received null student object.");
                return BadRequest("Student object cannot be null.");
            }
            if (string.IsNullOrEmpty(studentDTO.StudentName))
            {
                _logger.LogWarning("Student name is required.");
                return BadRequest("Student name is required.");
            }
            //var existingStudent = await _collegeDBContext.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

            var existingStudent = await _studentRepository.GetByFilterAsync(s => s.Id == id, true);// Using the repository to get the student by ID with no tracking

            if (existingStudent == null)
            {
                _logger.LogWarning($"Student with ID {id} not found for update.");
                return NotFound($"Student with ID {id} not found.");
            }

            var newStudent = _mapper.Map<Student>(studentDTO);// Using AutoMapper to map the StudentDTO to a Student entity


            //_collegeDBContext.Students.Update(newStudent);// Update the existing student with the new values
            //await _collegeDBContext.SaveChangesAsync();
            var StudentAfterUpdate = await _studentRepository.UpdateAsync(newStudent);// Using the repository to update the existing student in the database

            
            return NoContent(); // 204 No Content response
        }

        [HttpPatch("{id:int}", Name = "UpdateStudentPartially")]
        public async Task<ActionResult> UpdateStudentPartiallyAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            _logger.LogInformation($"UpdateStudentPartially method called with id: {id}");
            if (id <= 0)
            {
                _logger.LogWarning("Invalid student ID provided.");
                return BadRequest("Student ID must be greater than zero.");
            }
            if (patchDocument == null)
            {
                _logger.LogWarning("Received null patch document.");
                return BadRequest("Patch document cannot be null.");
            }
            //var existingStudent = await _collegeDBContext.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            var existingStudent = await _studentRepository.GetByFilterAsync(s => s.Id == id, true);// Using the repository to get the student by ID with no tracking
            if (existingStudent == null)
            {
                _logger.LogWarning($"Student with ID {id} not found for partial update.");
                return NotFound($"Student with ID {id} not found.");
            }

            var studentDTO = _mapper.Map<StudentDTO>(existingStudent);// Using AutoMapper to map the existing Student entity to a StudentDTO

            patchDocument.ApplyTo(studentDTO,ModelState);// Apply the patch document to the existing student

            existingStudent = _mapper.Map<Student>(studentDTO);// Using AutoMapper to map the updated StudentDTO back to a Student entity

            /*_collegeDBContext.Students.Update(existingStudent);
            await _collegeDBContext.SaveChangesAsync();*/

            var StudentAfterUpdate = await _studentRepository.UpdateAsync(existingStudent);// Using the repository to update the existing student in the database

            return NoContent(); // 204 No Content response
        }

        [HttpDelete("{id:int}", Name = "DeleteStudent")]
        public async Task<ActionResult> DeleteStudentAsync(int id)
        {
            _logger.LogInformation($"DeleteStudent method called with id: {id}");
            if (id <= 0)
            {
                _logger.LogWarning("Invalid student ID provided.");
                return BadRequest("Student ID must be greater than zero.");
            }

            //var existingStudent = _collegeDBContext.Students.FirstOrDefault(s => s.Id == id);
            var existingStudent = await _studentRepository.GetByFilterAsync(s => s.Id == id, false);// Using the repository to get the student by ID with tracking

            if (existingStudent == null)
            {
                _logger.LogWarning($"Student with ID {id} not found for deletion.");
                return NotFound($"Student with ID {id} not found.");
            }

            /*_collegeDBContext.Students.Remove(existingStudent);// Remove the existing student from the database

            await _collegeDBContext.SaveChangesAsync();*/

            bool IsDeleate = await _studentRepository.DeleteAsync(existingStudent);// Using the repository to delete the existing student from the database

            return NoContent(); // 204 No Content response
        }

        #endregion
    }
}

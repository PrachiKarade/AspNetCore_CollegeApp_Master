
namespace CoreWebApiSuperHero.Data.Repository
{
    public class StudentRepository: CollegeRepository<Student>, IStudentRepository // Inherit from generic repository and implement IStudentRepository
    {
       private readonly CollegeDBContext _collegeDBContext ;

        public StudentRepository(CollegeDBContext collegeDBContext):base(collegeDBContext) //base class CollegeRepository constructor
        {
            _collegeDBContext = collegeDBContext;
        }

        public async Task<List<Student>> GetStudentByGradeAsync(string grade)
        {
            return await _collegeDBContext.Students.ToListAsync();
        }
    }
}

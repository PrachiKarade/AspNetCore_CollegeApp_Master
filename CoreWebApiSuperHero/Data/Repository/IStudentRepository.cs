namespace CoreWebApiSuperHero.Data.Repository
{
    public interface IStudentRepository : ICollegeRepository<Student> // Inherit from common repository
    {
        Task<List<Student>> GetStudentByGradeAsync(string grade);
    }
}

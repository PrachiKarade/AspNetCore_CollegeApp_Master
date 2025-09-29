using Microsoft.Data.SqlClient;

namespace CoreWebApiSuperHero.Data.Repository
{
    public class UserRepository : CollegeRepository<User>, IUserRepository
    {
        private readonly CollegeDBContext _collegeDBContext;
        public UserRepository(CollegeDBContext collegeDBContext) : base(collegeDBContext)
        {
            _collegeDBContext = collegeDBContext;            
        }

        public async Task<bool> CreateUserBySPAsync(User userdto)
        {
            var result = await _collegeDBContext.Database.ExecuteSqlInterpolatedAsync($"EXEC sp_InsertUser @UserName = {userdto.UserName},@Password = {userdto.Password},@PasswordSalt = {userdto.PasswordSalt}");                           

            if(result <= 0)
                return false;

            return true;
        }
    }
}

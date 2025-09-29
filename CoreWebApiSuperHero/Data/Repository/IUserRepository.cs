namespace CoreWebApiSuperHero.Data.Repository
{
    public interface IUserRepository : ICollegeRepository<User> // Inherit from common repository
    {
        public Task<bool> CreateUserBySPAsync(User userdto);
    }
}

using CoreWebApiSuperHero.Data;
using CoreWebApiSuperHero.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApiSuperHero.Services
{
    public interface IUserService 
    {
        public (string PasswordHash, string Salt) CreatePasswordHashWithSalt(string password);

        public Task<List<UserDTO>> GetAllUsersAsync();

        public Task<UserDTO> GetUserByIdAsync(int UserId);

        public Task<bool> CreateUserAsync(UserDTO userdto);

        public Task<bool> UpdateUserAsync(UserDTO userdto);

        public Task<bool> DeleteUserAsync(int UserId);

        public Task<bool> CreateUserBySPAsync(UserDTO userdto);
    }
}

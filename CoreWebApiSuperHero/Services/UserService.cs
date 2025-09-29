using System.Data.OleDb;
using System.Linq.Expressions;
using System.Security.Cryptography;
using CoreWebApiSuperHero.Data;
using CoreWebApiSuperHero.Data.Repository;
using CoreWebApiSuperHero.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace CoreWebApiSuperHero.Services
{
    public class UserService : IUserService 
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public (string PasswordHash ,string Salt) CreatePasswordHashWithSalt(string password)
        {
           // create salt
            var salt = new byte[128/8];
            using (var rng = RandomNumberGenerator.Create()) 
            {
                rng.GetBytes(salt);
            }
            //create password hash   
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));
            return (hash, Convert.ToBase64String(salt));
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users =  await _userRepository.GetAllAsync();

            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserByIdAsync(int UserId)
        {
            if(UserId <= 0)
                throw new ArgumentException("UserId must be greater than 0");

            var user = await _userRepository.GetByFilterAsync(user => user.UserId == UserId, false);

            return _mapper.Map<UserDTO>(user);

        }

        public async Task<bool> CreateUserAsync(UserDTO userdto) 
        {
            /*https://www.youtube.com/watch?v=W7aRQLO62WI&list=PL3ewn8T-zRWgO-GAdXjVRh-6thRog6ddg&index=109
            //https://www.youtube.com/watch?v=35xyM_oOUyg
            //https://www.youtube.com/watch?v=V5pdycHYBvk&list=PL8p2I9GklV45tiZ9F_rbWnx_WHC0GCShX&index=5
            //https://platform.openai.com/docs/guides/image-generation
            */
            /* old way
            if(userdto == null)
                throw new ArgumentNullException("UserDTO object is null");*/

            //new way in .net 6
            ArgumentNullException.ThrowIfNull($"arguments of {nameof(userdto)} are null");

            if(String.IsNullOrEmpty(userdto.UserName) || String.IsNullOrEmpty(userdto.Password) )
            {
               throw new ArgumentNullException("Password is null");
            }
            
            var existingUser = await _userRepository.GetAllByFilterAsync(user => user.UserName == userdto.UserName);

            if(existingUser != null)
            {
                User newUser = _mapper.Map<User>(userdto);

                newUser.Createdate = DateTime.UtcNow;
                newUser.Updatedate = DateTime.UtcNow;
                newUser.IsActive = true;
                newUser.IsDeleated = false;

                var passwordHashWithSalt = CreatePasswordHashWithSalt(userdto.Password);

                newUser.Password = passwordHashWithSalt.PasswordHash;
                newUser.PasswordSalt = passwordHashWithSalt.Salt;

                await _userRepository.CreateAsync(newUser);
                   
            }            
            return true;
        }

        public async Task<bool> UpdateUserAsync(UserDTO userdto)
        {
            if(userdto == null || userdto.UserId <= 0)
                throw new ArgumentNullException("arguments of UserDTO are null");

            var existingUser = await _userRepository.GetByFilterAsync(user => user.UserId == userdto.UserId, true);

            if(existingUser == null)
                throw new KeyNotFoundException($"User not found with UserId: {userdto.UserId}");

            var userToUpdate = _mapper.Map<User>(userdto);

            userToUpdate.Updatedate = DateTime.UtcNow;

            if (!String.IsNullOrEmpty(userdto.Password))
            {
                var passwordHashWithSalt = CreatePasswordHashWithSalt(userdto.Password);

                userToUpdate.Password = passwordHashWithSalt.PasswordHash;
                userToUpdate.PasswordSalt = passwordHashWithSalt.Salt;
            }

            await _userRepository.UpdateAsync(userToUpdate);

            return true;

        }

        public async Task<bool> DeleteUserAsync(int UserId)
        {
            if(UserId <= 0)
                throw new ArgumentException("UserId must be greater than 0");

            var existingUser = await _userRepository.GetByFilterAsync(user => user.UserId == UserId, true);

            if(existingUser == null)
                throw new KeyNotFoundException($"User not found with UserId: {UserId}");

            existingUser.IsDeleated = true;
            existingUser.IsActive = false;
            existingUser.Updatedate = DateTime.UtcNow;

            await _userRepository.UpdateAsync(existingUser);

            return true;            
        }

        public async Task<bool> CreateUserBySPAsync(UserDTO userdto)
        {
            ArgumentNullException.ThrowIfNull($"arguments of {nameof(userdto)} are null");

            if (String.IsNullOrEmpty(userdto.UserName) || String.IsNullOrEmpty(userdto.Password))
            {
                throw new ArgumentNullException("Password is null");
            }

            var existingUser = await _userRepository.GetAllByFilterAsync(user => user.UserName == userdto.UserName);

            if (existingUser != null)
            {
                User newUser = _mapper.Map<User>(userdto);

                newUser.Createdate = DateTime.UtcNow;
                newUser.Updatedate = DateTime.UtcNow;
                newUser.IsActive = true;
                newUser.IsDeleated = false;

                var passwordHashWithSalt = CreatePasswordHashWithSalt(userdto.Password);

                newUser.Password = passwordHashWithSalt.PasswordHash;
                newUser.PasswordSalt = passwordHashWithSalt.Salt;

                //await _userRepository.CreateAsync(newUser);
               var result =  await _userRepository.CreateUserBySPAsync(newUser);               
            }           
           return true; 
        }

    }
}

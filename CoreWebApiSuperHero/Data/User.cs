
using System.ComponentModel.DataAnnotations;

namespace CoreWebApiSuperHero.Data
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }
        public int UserType { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleated { get; set; }

        public DateTime Createdate { get; set; }

        public DateTime Updatedate { get; set; }

        public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; }

    }
}

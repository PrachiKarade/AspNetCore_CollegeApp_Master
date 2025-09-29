namespace CoreWebApiSuperHero.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int UserType { get; set; }

        public bool IsActive { get; set; }
       
    }
}

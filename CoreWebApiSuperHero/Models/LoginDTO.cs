using System.ComponentModel.DataAnnotations;

namespace CoreWebApiSuperHero.Models
{
    public class LoginDTO
    {
        [Required]
        public string userName {  get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string userRole { get; set; }

        [Required]
        public string policy { get; set; }
    }
}

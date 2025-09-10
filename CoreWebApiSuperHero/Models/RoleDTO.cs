using System.ComponentModel.DataAnnotations;
using CoreWebApiSuperHero.Data;

namespace CoreWebApiSuperHero.Models
{
    public class RoleDTO
    {
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

       
    }
}

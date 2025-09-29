using CoreWebApiSuperHero.Data;

namespace CoreWebApiSuperHero.Models
{
    public class RolePrivilegeDTO
    {
        public int RolePrivilegeId { get; set; }

        public string RolePrivilegeName { get; set; }

        public string Description { get; set; }

        public int RoleId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleated { get; set; }

        public DateTime Createdate { get; set; }

        public DateTime Updatedate { get; set; }

        
    }
}

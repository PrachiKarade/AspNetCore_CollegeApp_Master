namespace CoreWebApiSuperHero.Data
{
    public class Role
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleated { get; set; }

        public DateTime Createdate { get; set; }

        public DateTime Updatedate { get; set; }

        public virtual ICollection<RolePrivilege> RolePrivileges { get; set; }

        public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; }

    }
}

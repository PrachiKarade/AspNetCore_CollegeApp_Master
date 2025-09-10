using CoreWebApiSuperHero.Data.Config;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoreWebApiSuperHero.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } //this property represents the collection of User entities (Users Table) in the database

        public DbSet<Role> Roles { get; set; }//this property represents the collection of Role entities (Roles Table) in the database

        public DbSet<RolePrivilege> RolePrivileges { get; set; }//this property represents the collection of Role entities (Roles Table) in the database

        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }//this property represents the collection of Role entities (Roles Table) in the database
        public DbSet<Student> Students { get; set; } //this property represents the collection of Student entities (Students Table) in the database

        public DbSet<Department> Departments { get; set; } //this property represents the collection of Student entities (Students Table) in the database

        protected override void OnModelCreating(ModelBuilder modelBuilder)// This method is used to configure the model and its relationships using the Fluent API.
        {
            base.OnModelCreating(modelBuilder);

            //below commented code can be written in a separate class file eg.StudentConfig.cs and call it here to register that file.
            // this is udsually done to keep the DbContext class clean and maintainable.
            // if multiple entities are configured, it is better to create separate configuration classes for each entity.
            //  This method is called by the runtime to configure the model for the context.

            /*modelBuilder.Entity<Student>().HasData(new List<Student>()
           {
               new Student
               {
                   Id = 1,
                   StudentName = "Prachi",
                   Email = "prachi.karade@gmail.com",
                   Address = "India",
                   DOB = new DateTime(1982, 06, 12)
               },
               new Student
               {
                   Id = 2,
                   StudentName = "Sarvada",
                   Email = "sarvada.jothangiya@gmail.com",
                   Address = "India",
                   DOB = new DateTime(2014, 07,15)
               }
           });

           modelBuilder.Entity<Student>( entitity => {                 

               entitity.HasKey(e => e.Id); // This sets the primary key for the Student entity to the Id property
               entitity.Property(e => e.StudentName).IsRequired().HasMaxLength(50); // This sets the StudentName property to be required and have a maximum length of 50 characters
               entitity.Property(e => e.Email).IsRequired(false).HasMaxLength(250); // This sets the Email property to have a maximum length of 250 characters
               entitity.Property(e => e.Address).IsRequired(false).HasMaxLength(500); // This sets the Address property to have a maximum length of 500 characters
           });*/

            //Table 1 : Students
            modelBuilder.ApplyConfiguration(configuration: new StudentConfig()); // This applies the configuration defined in the StudentConfig class to the Student entity

            //Table 2 : Departments
            modelBuilder.ApplyConfiguration(configuration: new DepartmentConfig()); // This applies the configuration defined in the DepartmentConfig class to the Department entity

            //Table 3 : Users
            modelBuilder.ApplyConfiguration(configuration: new UserConfig()); // This applies the configuration defined in the DepartmentConfig class to the Department entity

            //Table 4 : Roles
            modelBuilder.ApplyConfiguration(configuration: new RoleConfig()); // This applies the configuration defined in the DepartmentConfig class to the Department entity

            modelBuilder.ApplyConfiguration(configuration: new RolePrivilegeConfig()); // This applies the configuration defined in the DepartmentConfig class to the Department entity

            modelBuilder.ApplyConfiguration(configuration: new UserRoleMappingConfig()); // This applies the configuration defined in the DepartmentConfig class to the Department entity

        }
    }
}

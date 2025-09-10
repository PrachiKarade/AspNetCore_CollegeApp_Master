using CoreWebApiSuperHero.Data;

namespace CoreWebApiSuperHero.Models
{
    public class DepartmentDTO
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }
        public string? Description { get; set; }

       // public virtual ICollection<Student>? Students { get; set; }
    }
}

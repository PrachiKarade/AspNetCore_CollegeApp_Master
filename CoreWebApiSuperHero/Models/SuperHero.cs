using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CoreWebApiSuperHero.Models
{
    public class SuperHero
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Hero name is required.")]
        [StringLength(30, ErrorMessage = "Hero name cannot be longer than 30 characters.")]// This attribute is used to specify that the Name property is required and cannot be longer than 100 characters
        public string Name { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [ValidateNever] // This attribute is used to prevent validation of this property in ASP.NET Core MVC
        public string? Place { get; set; }
        
    }
}

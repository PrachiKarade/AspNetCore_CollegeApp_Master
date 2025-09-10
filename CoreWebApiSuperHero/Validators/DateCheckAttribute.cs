using System.ComponentModel.DataAnnotations;

namespace CoreWebApiSuperHero.Validators
{
    public class DateCheckAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var date = (DateTime?)value;
            if ( date < DateTime.Now)
            {
                return new ValidationResult(ErrorMessage="Date must be greater than or eqaul to Today .");
            }
            return ValidationResult.Success;
        }
    }   
}

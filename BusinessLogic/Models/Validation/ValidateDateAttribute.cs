using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models.Validation
{
    public class ValidateDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            DateTime date = Convert.ToDateTime(value);
            if(date.AddYears(35) < DateTime.Now) {

                return new ValidationResult("Abit old unfortunetly");
            }

            if(date.AddYears(16) > DateTime.Now)
            {
                return new ValidationResult("Not quite old enough");
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}

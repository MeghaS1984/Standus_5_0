using System.ComponentModel.DataAnnotations;

namespace Standus_5_0.ValidationAttributes
{
    public class EAndDValidation : ValidationAttribute
    {
        public EAndDValidation() {
            ErrorMessage = "";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return ValidationResult.Success; 
        }
    }
}

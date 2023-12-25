using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MyBGList.Attributes
{
    public class LettersOnlyValidatorAttribute : ValidationAttribute
    {
        public LettersOnlyValidatorAttribute()
            : base("Value must contain only letters (no spaces, digits, or other chars)") { }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            var strValue = value as string;
            if (!string.IsNullOrEmpty(strValue) && 
                new Regex("^[a-zA-Z]+$").IsMatch(strValue))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}

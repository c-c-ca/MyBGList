using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MyBGList.Attributes
{
    public class LettersOnlyValidatorAttribute : ValidationAttribute
    {
        public bool UseRegex { get; set; } = false;

        public LettersOnlyValidatorAttribute()
            : base("Value must contain only letters (no spaces, digits, or other chars)") { }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            var strValue = value as string;
            if (!string.IsNullOrEmpty(strValue))
            {
                if (UseRegex)
                {
                    if (new Regex("^[a-zA-Z]+$").IsMatch(strValue))
                    {
                        return ValidationResult.Success;
                    }
                }
                else if (strValue.All(char.IsLetter))
                {
                    return ValidationResult.Success;
                }
                
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MyBGList.Attributes
{
    public class SortColumnValidatorAttribute : ValidationAttribute
    {
        public Type EntityType { get; set; }

        public SortColumnValidatorAttribute(Type entityType)
            : base("Value must match an existing column")
        {
            EntityType = entityType;
        }

        protected override ValidationResult? IsValid(
            object? value, 
            ValidationContext validationContext)
        {
            if (EntityType != null)
            {
                var strValue = value as string;
                if (!string.IsNullOrEmpty(strValue) &&
                    EntityType.GetProperties().Any(p => 
                        string.Equals(p.Name, strValue, StringComparison.OrdinalIgnoreCase)))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}

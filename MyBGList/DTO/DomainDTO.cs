using MyBGList.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBGList.DTO
{
    public class DomainDTO : IValidatableObject
    {
        [Required]
        public int Id { get; set; }

        [LettersOnlyValidator(UseRegex = true)]
        public string? Name { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            return Id == 3 || Name == "Wargames" ?
                new ValidationResult[0] :
                new [] { 
                    new ValidationResult("Id and/or Name values must match an allowed Domain.") 
                };
        }
    }
}

using MyBGList.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBGList.DTO
{
    public class DomainDTO
    {
        [Required]
        public int Id { get; set; }

        [LettersOnlyValidator]
        public string? Name { get; set; }
    }
}

using SampleTest.Domain.Enums;
using SampleTest.Domain.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleTest.Domain.Models
{
    [Table("Client")]
    public class ClientModel : TrackableEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Column(TypeName = "timestamp")]
        public DateTime BirthDate { get; set; }
        [Required]
        public string CPF { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public GenderEnum Gender { get; set; }
        [Required]
        public double MonthRemuneration { get; set; } = 0;
    }
}

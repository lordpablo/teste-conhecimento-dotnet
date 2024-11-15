using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleTest.Domain.Models.Shared
{
    public abstract class Entity
    {
        [Key]
        [DatabaseGenerated
         (DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        [Required]
        public int Id { get; set; }
    }
}

using SampleTest.Domain.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleTest.Domain.Models
{
    [Table("Account")]
    public class AccountModel : TrackableEntity
    {
        [Required]
        public double Balance { get; set; } = 0;

        [Required]
        public double Overdraft { get; set; } = 0;

        [Required]
        public string Agency { get; set; }

        [Required]
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public ClientModel Client { get; set; }
    }
}


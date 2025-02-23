using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        public required string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public virtual Order Order { get; set; } = null!;

    }
}

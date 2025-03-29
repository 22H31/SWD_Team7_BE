using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class PaymentInformation
    {
        [Key]  // Định nghĩa khóa chính
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required string OrderType { get; set; }
        public double Amount { get; set; }
        public required string OrderDescription { get; set; }
        public required string Name { get; set; }
    }
}
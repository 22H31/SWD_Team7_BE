using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Dtos.Order
{
    public class CustomerRefundUpdateDto
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public string BankAccountNumber { get; set; } = string.Empty;

        [Required]
        public string BankName { get; set; } = string.Empty;

        [Required]
        public string AccountHolderName { get; set; } = string.Empty;
    }
}

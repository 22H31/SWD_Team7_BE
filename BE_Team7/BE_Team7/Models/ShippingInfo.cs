using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class ShippingInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ShippingInfoId { get; set; }
        [ForeignKey("User")]
        public required string Id { get; set; }
        public virtual User User { get; set; } = null!;
        public required string AddressType { get; set; }
        public required string LastName { get; set; }
        public required string FirstName { get; set; }
        public required string ShippingPhoneNumber { get; set; }
        public required string Province { get; set; }
        public required string District { get; set; }
        public required string Commune { get; set; }
        public required string AddressDetail { get; set; }
        public required string ShippingNote { get; set; }
        public required bool DefaultAddress { get; set; }
        public DateTime? ShippingInfoCreateAt { get; set; }
    }
}

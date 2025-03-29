namespace BE_Team7.Dtos.ShippingInfo
{
    public class UpdateShippingInfoDto
    {
        public required string AddressType { get; set; }
        public required string LastName { get; set; }
        public required string FirstName { get; set; }
        public required string ShippingPhoneNumber { get; set; }
        public required string Province { get; set; }
        public required string District { get; set; }
        public required string Commune { get; set; }
        public required string AddressDetail { get; set; }
        public required string ShippingNote { get; set; }
    }
}

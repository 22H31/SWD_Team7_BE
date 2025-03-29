namespace BE_Team7.Dtos.Voucher
{
    public class VoucherResponseDto
    {
        public required Guid VoucherId { get; set; }
        public required string VoucherName { get; set; } = string.Empty;
        public required string VoucherDescription { get; set; } = string.Empty;
        public required DateTime VoucherEndDate { get; set; }
        public int VoucherQuantity { get; set; }
        public decimal VoucherRate { get; set; }
    }
}

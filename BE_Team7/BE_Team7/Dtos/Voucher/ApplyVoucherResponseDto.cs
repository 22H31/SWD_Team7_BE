namespace BE_Team7.Dtos.Voucher
{
    public class ApplyVoucherResponseDto
    {
        public Guid VoucherId { get; set; }
        public required string VoucherName { get; set; } = string.Empty;
        public decimal VoucherRate { get; set; }
        public List<Guid> ApplicableProductIds { get; set; } = new List<Guid>();
    }
}

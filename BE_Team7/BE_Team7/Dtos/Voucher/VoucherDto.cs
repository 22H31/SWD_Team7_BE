namespace BE_Team7.Dtos.Voucher
{
    public class VoucherDto
    {
        public required Guid VoucherId { get; set; }
        public required string VoucherName { get; set; } = string.Empty;
        public required string VoucherDescription { get; set; } = string.Empty;
        public decimal VoucherRate { get; set; }
        public int VoucherQuantity { get; set; }
        public required DateTime VoucherStartDate { get; set; }
        public required DateTime VoucherEndDate { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }
    }
}

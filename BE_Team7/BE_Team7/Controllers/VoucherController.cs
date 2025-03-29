using BE_Team7.Dtos.Voucher;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.AspNetCore.Mvc;

namespace BE_Team7.Controllers
{
    [Route("api/voucher")]
    [ApiController]
    public class VoucherController : Controller
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherController(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoucherDto>>> GetAllVouchers()
        {
            var vouchers = await _voucherRepository.GetAllVouchersAsync();
            var voucherDtos = vouchers.Select(v => new VoucherDto
            {
                VoucherId = v.VoucherId,
                VoucherName = v.VoucherName,
                VoucherDescription = v.VoucherDescription,
                VoucherRate = v.VoucherRate,
                VoucherQuantity = v.VoucherQuantity,
                VoucherStartDate = v.VoucherStartDate,
                VoucherEndDate = v.VoucherEndDate,
                CategoryId = v.CategoryId,
                BrandId = v.BrandId
            }).ToList();

            return Ok(voucherDtos);
        }
        [HttpGet("{voucherId}")]
        public async Task<ActionResult<VoucherDto>> GetVoucher(Guid voucherId)
        {
            var voucher = await _voucherRepository.GetVoucherByIdAsync(voucherId);
            if (voucher == null)
            {
                return NotFound();
            }

            var voucherDto = new VoucherDto
            {
                VoucherId = voucher.VoucherId,
                VoucherName = voucher.VoucherName,
                VoucherDescription = voucher.VoucherDescription,
                VoucherRate = voucher.VoucherRate,
                VoucherQuantity = voucher.VoucherQuantity,
                VoucherStartDate = voucher.VoucherStartDate,
                VoucherEndDate = voucher.VoucherEndDate,
                CategoryId = voucher.CategoryId,
                BrandId = voucher.BrandId
            };

            return Ok(voucherDto);
        }
        [HttpPost]
        public async Task<ActionResult<VoucherDto>> CreateVoucher([FromBody] CreateVoucherDto createVoucherDto)
        {
            var voucher = new Voucher
            {
                VoucherName = createVoucherDto.VoucherName,
                VoucherDescription = createVoucherDto.VoucherDescription,
                VoucherRate = createVoucherDto.VoucherRate,
                VoucherQuantity = createVoucherDto.VoucherQuantity,
                VoucherStartDate = createVoucherDto.VoucherStartDate,
                VoucherEndDate = createVoucherDto.VoucherEndDate,
                CategoryId = createVoucherDto.CategoryId,
                BrandId = createVoucherDto.BrandId
            };

            var createdVoucher = await _voucherRepository.AddVoucherAsync(voucher);

            var voucherDto = new VoucherDto
            {
                VoucherId = createdVoucher.VoucherId,
                VoucherName = createdVoucher.VoucherName,
                VoucherDescription = createdVoucher.VoucherDescription,
                VoucherRate = createdVoucher.VoucherRate,
                VoucherQuantity = createdVoucher.VoucherQuantity,
                VoucherStartDate = createdVoucher.VoucherStartDate,
                VoucherEndDate = createdVoucher.VoucherEndDate,
                CategoryId = createdVoucher.CategoryId,
                BrandId = createdVoucher.BrandId
            };

            return CreatedAtAction(nameof(GetVoucher), new { voucherId = voucherDto.VoucherId }, voucherDto);
        }

        [HttpPut("{voucherId}")]
        public async Task<IActionResult> UpdateVoucher(Guid voucherId, [FromBody] UpdateVoucherDto updateVoucherDto)
        {
            var voucher = await _voucherRepository.GetVoucherByIdAsync(voucherId);
            if (voucher == null)
            {
                return NotFound();
            }

            voucher.VoucherName = updateVoucherDto.VoucherName;
            voucher.VoucherDescription = updateVoucherDto.VoucherDescription;
            voucher.VoucherRate = updateVoucherDto.VoucherRate;
            voucher.VoucherQuantity = updateVoucherDto.VoucherQuantity;
            voucher.VoucherStartDate = updateVoucherDto.VoucherStartDate;
            voucher.VoucherEndDate = updateVoucherDto.VoucherEndDate;
            voucher.CategoryId = updateVoucherDto.CategoryId;
            voucher.BrandId = updateVoucherDto.BrandId;

            await _voucherRepository.UpdateVoucherAsync(voucher);

            return Ok(new
            {
                message = "Cập nhật voucher thành công!",
                updatedVoucher = voucher
            });
        }

        [HttpDelete("{voucherId}")]
        public async Task<IActionResult> DeleteVoucher(Guid voucherId)
        {
            var result = await _voucherRepository.DeleteVoucherAsync(voucherId);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new
            {
                message = "Xóa voucher thành công!"          
            });
        }
        [HttpPost("GetVouchersByProductIds")]
        public async Task<ActionResult<List<VoucherResponseDto>>> GetVouchersByProductIds([FromBody] ProductIdsRequestDto productIdsRequest)
        {
            var vouchers = await _voucherRepository.GetVouchersByProductIdsAsync(productIdsRequest.ProductIds);
            return Ok(vouchers);
        }

        [HttpPost("ApplyVoucher")]
        public async Task<ActionResult<ApplyVoucherResponseDto>> ApplyVoucher(Guid voucherId)
        {
            try
            {
                var response = await _voucherRepository.ApplyVoucherAsync(voucherId);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}

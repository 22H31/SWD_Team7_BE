using AutoMapper;
using BE_Team7.Dtos.Brand;
using BE_Team7.Dtos.Product;
using BE_Team7.Helpers;
using BE_Team7.Shared.ErrorModel;
using BE_Team7.Shared.Extensions;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Interfaces.Service.Contracts;
using BE_Team7.Models;
using GarageManagementAPI.Shared.ResultModel;
using Microsoft.AspNetCore.Mvc;
using BE_Team7.Dtos.Blog;
using Microsoft.AspNetCore.Authorization;

namespace BE_Team7.Controllers
{
    [Route("api/brand")]
    [ApiController]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepo;
        private readonly IMediaService _mediaService;

        public BrandController(AppDbContext context, IBrandRepository brandRepo, IMapper mapper,IMediaService mediaService)
        {
            _brandRepo = brandRepo;
            _context = context;
            _mapper = mapper;
            _mediaService = mediaService;

        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet]
        public async Task<IActionResult> GetAllBrand()
        {
            var brandDto = await _brandRepo.GetBrandAsync();
            return Ok(brandDto);
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet("{brandId}")]
        public async Task<IActionResult> GetBrandById([FromRoute] string brandId)
        {
            var brand = await _brandRepo.GetBrandById(brandId);
            if (brand == null)
            {
                return NotFound(new { message = "brand không tồn tại." });
            }
            var blogDto = _mapper.Map<BrandDetailDto>(brand);
            return Ok(blogDto);
        }
        //[Authorize(Policy = "RequireStaffSale")]
        [HttpPost]
        public async Task<IActionResult> CreateNewBrand([FromBody] CreateBrandRequestDto createBrandRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var brandModel = _mapper.Map<Brand>(createBrandRequestDto);
            await _brandRepo.CreateBrandAsync(brandModel);

            return CreatedAtAction(nameof(GetBrandById), new { brandId = brandModel.BrandId }, _mapper.Map<BrandDto>(brandModel));
        }
        //[Authorize(Policy = "RequireStaffSale")]
        [HttpPut]
        [Route("{brandId:Guid}")]
        public async Task<IActionResult> UpdateBrand([FromRoute] Guid brandId, [FromBody] UpdateBrandRequestDto updateBrandRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Brand>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var brandModel = await _brandRepo.UpdateBrandAsync(brandId, updateBrandRequestDto);

            if (!brandModel.Success)
                return NotFound(brandModel);
            return Ok(brandModel);
        }
        //[Authorize(Policy = "RequireStaffSaleOrAdmin")]
        [HttpDelete("{brandId:Guid}")]
        public async Task<IActionResult> DeleteBrand([FromRoute] Guid brandId)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Brand>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var brandModel = await _brandRepo.DeleteBrandAsync(brandId);
            if (!brandModel.Success)
                return NotFound(brandModel);
            return Ok(brandModel);
        }
        //[Authorize(Policy = "RequireStaffSale")]
        [HttpPost("{brandId}/brand_avartar_images", Name = "UploadBrandAvartarImage")]
        public async Task<IActionResult> UploadBrandAvartarImage(Guid brandId, [FromForm] List<IFormFile> fileDtos)
        {
            var brandExists = await _brandRepo.GetBrandById(brandId.ToString());
            if (brandExists == null)
            {
                return NotFound(new { message = "Brand không tồn tại." });
            }
            if (fileDtos == null || !fileDtos.Any())
            {
                return BadRequest("No files were uploaded.");
            }
            var uploadBrandAvartarImages = new List<object>();
            foreach (var fileDto in fileDtos)
            {
                var uploadFileResult = await _mediaService.UploadAvatarBrandImageAsync(fileDto);

                if (!uploadFileResult.IsSuccess)
                    return BadRequest(Result.Failure(HttpStatusCode.BadRequest, uploadFileResult.Errors));

                var imgTuple = uploadFileResult.GetValue<(string? publicId, string? absoluteUrl)>();

                var updateResult = await _brandRepo.UploadBrandAvartarImgAsync(brandId, imgTuple.publicId!, imgTuple.absoluteUrl!);

                if (!updateResult.Success) return BadRequest(Result.Failure(HttpStatusCode.BadRequest, new List<ErrorsResult>()));

            }
            return Ok();
        }
    }
}

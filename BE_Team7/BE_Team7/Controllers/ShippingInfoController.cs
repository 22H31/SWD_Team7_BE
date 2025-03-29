using Microsoft.AspNetCore.Mvc;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Dtos.ShippingInfo;
using BE_Team7.Models;
using BE_Team7.Repository;
using Microsoft.AspNetCore.Authorization;


namespace BE_Team7.Controllers
{
    [Route("api/shippingInfo")]
    [ApiController]
    public class ShippingInfoController : Controller
    {
        private readonly IShippingInfoRepository _shippingInfoRepo;

        public ShippingInfoController(IShippingInfoRepository shippingInfoRepository)
        {
            _shippingInfoRepo = shippingInfoRepository;
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpPost]
        public async Task<IActionResult> CreateShippingInfo([FromBody] ShippingInfoDto shippingInfoDto)
        {
            var response = await _shippingInfoRepo.CreateShippingInfoAsync(shippingInfoDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpPut("{shippingInfoId}")]
        public async Task<IActionResult> UpdateShippingInfo(Guid shippingInfoId, [FromBody] UpdateShippingInfoDto shippingInfoDto)
        {
            var response = await _shippingInfoRepo.UpdateShippingInfoAsync(shippingInfoId, shippingInfoDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpDelete("{shippingInfoId}")]
        public async Task<IActionResult> DeleteShippingInfo(Guid shippingInfoId)
        {
            var response = await _shippingInfoRepo.DeleteShippingInfoAsync(shippingInfoId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetShippingInfosByUserId(string id)
        {
            var response = await _shippingInfoRepo.GetShippingInfosByUserIdAsync(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpPut("default/{id}/{shippingInfoId}")]
        public async Task<IActionResult> UpdateDefaultAddress(string id, Guid shippingInfoId)
        {
            var response = await _shippingInfoRepo.UpdateDefaultAddressAsync(id, shippingInfoId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet("{shippingInfoId}")]
        public async Task<IActionResult> GetShippingInfo(Guid shippingInfoId)
        {
            var response = await _shippingInfoRepo.GetShippingInfoByIdAsync(shippingInfoId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}

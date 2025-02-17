using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DatabaseController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("check-connection")]
        public IActionResult CheckDatabaseConnection()
        {
            if (_context.Database.CanConnect())
            {
                return Ok("✅ Kết nối thành công!");
            }
            else
            {
                return StatusCode(500, "❌ Kết nối thất bại!");
            }
        }
    }
}

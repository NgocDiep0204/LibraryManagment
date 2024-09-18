using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_librarymanagment.Data;

namespace api_librarymanagment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
   
    public class PostitionsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public PostitionsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]

        public async Task<IActionResult> GetAllPostion()
        {
            var positions = await context.Positions.ToListAsync();
            return Ok(positions);
        }
    }
}

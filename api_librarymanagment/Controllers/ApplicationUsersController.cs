using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_librarymanagment.Data;

namespace api_librarymanagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        public ApplicationUsersController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStaff()
        {
            var staffs = await context.ApplicationUsers
                .ToListAsync();
            return Ok(staffs);
        }
    }
}

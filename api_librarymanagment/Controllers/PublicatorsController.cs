using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_librarymanagment.Data;
using api_librarymanagment.Data.DTOs;
using api_librarymanagment.Models.Library;

namespace api_librarymanagment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PublicatorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public PublicatorsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPublicators()
        {
            var publicators = await context.Publicators.ToListAsync();
            return Ok(publicators);
        }

    }
}

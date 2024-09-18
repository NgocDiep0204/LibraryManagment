using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_librarymanagment.Data;
using api_librarymanagment.Data.DTOs;
using api_librarymanagment.Models.Library;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.AspNetCore.Authorization;

namespace api_librarymanagment.Controllers
{
    [Authorize (Roles = "Admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AthorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public AthorsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await context.Authors.ToListAsync();
            return Ok(authors);

        }
    }
}

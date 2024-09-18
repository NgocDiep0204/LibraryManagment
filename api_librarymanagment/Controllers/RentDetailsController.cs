using api_librarymanagment.Data;
using api_librarymanagment.Data.DTOs;
using api_librarymanagment.Models.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_librarymanagment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RentDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public RentDetailsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRentDetail()
        {
            var rentdetails = await context.RentDetails
                                      .Include(c => c.BookIdNavigation)
                                      .Include(b => b.RentIdNavigation)
                                      .ToListAsync();
            return Ok(rentdetails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRentDetailById(string id)
        {
            var rentdetail = await context.RentDetails
                                      .Include(c => c.BookIdNavigation)
                                      .Include(b => b.RentIdNavigation)
                                      .Where(c => c.RentId == id)
                                      .AsNoTracking()
                                      .ToListAsync();
           
            if (!rentdetail.Any())
            {
                return NotFound();
            }

            return Ok(rentdetail);

        }

        [HttpPost]
        public async Task<IActionResult> CreateRentDetail([FromBody] RentDetailDTO rentDetail)
        {
            try
            {
                var newRentDetail = new RentDetail
                {
                    RentId = rentDetail.RentId,
                    BookId = rentDetail.BookId,
                    Quantity = rentDetail.Quantity
                };
                await context.RentDetails.AddAsync(newRentDetail);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception details
                // Optionally, inspect the inner exception for more specific details
                Console.WriteLine(ex.InnerException?.Message);
            }
            return Ok();
        }

       

    }
}

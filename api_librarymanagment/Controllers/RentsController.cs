using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_librarymanagment.Data;
using api_librarymanagment.Data.DTOs;
using api_librarymanagment.Models.Library;
using api_librarymanagment.Functions;

namespace api_librarymanagment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly int pageSize = 10;
        public RentsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{pageNumber}")]
        public async Task<IActionResult> GetAllRentials(int pageNumber = 1)
        {
            var rentials = await context.Rents
                .Include(b => b.UserIdNavigation)
                .Include(c => c.StudentIdNavigation)
                .AsNoTracking()
                .ToListAsync();
            var pagedRentials = PagedList<Rent>.Create(rentials, pageNumber, pageSize);
           return Ok(new
            {
                pagedRentials,
                pagedRentials.TotalPages
            });
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetRentialsByNameStudent(string name, int pageNumber)
        {
            var rentials = await context.Rents
                        .Include(r => r.StudentIdNavigation)
                        .Where(r => r.StudentIdNavigation != null && EF.Functions.Collate(r.StudentIdNavigation.Name, "SQL_Latin1_General_CP1_CI_AI").Contains(name))
                        .AsNoTracking()
                        .ToListAsync();
            var pagedRentials = PagedList<Rent>.Create(rentials, pageNumber, pageSize);
            return Ok(new
            {
                pagedRentials,
                pagedRentials.TotalPages
            });
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetRentById(string id)
        {
            var rent = await context.Rents
                .Include(b => b.UserIdNavigation)
                .Include(c => c.StudentIdNavigation)
                .Where(r => r.RentId == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(rent);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRent([FromBody] RentDTO rent)
        {
            var newRent = new Rent
            {
                RentId = rent.RentId,
                StudentId = rent.StudentId,
                UserId = rent.UserId,
                RentDate = rent.RentDate,
                ReturnDate = rent.ReturnDate,
                Status = rent.Status,
                TotalBook = rent.TotalBook
            };
            context.Rents.Add(newRent);
            await context.SaveChangesAsync();
            return Ok(newRent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRentStatus(string id, [FromBody] RentDTO rent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var rentToUpdate = await context.Rents
                .Where(r => r.RentId == id)
                .FirstOrDefaultAsync();
            if (rentToUpdate == null)
            {
                return NotFound();
            }
            rentToUpdate.RentId = rent.RentId;
           // rentToUpdate.StudentId = rent.StudentId;
            rentToUpdate.Status = rent.Status;
            
            await context.SaveChangesAsync(); // Save changes to the database
            return Ok(rentToUpdate);
        }

        [HttpGet]
        public async Task<IActionResult> GetCountRentsByMonth()
        {
            int currentYear = DateTime.Now.Year;

            var months = Enumerable.Range(1, 12).Select(month => new { MonthNumber = month });

            var query = from m in months
                        join r in context.Rents on m.MonthNumber equals (r.RentDate.HasValue ? r.RentDate.Value.Month : 0) into rentGroup
                        from subRent in rentGroup.DefaultIfEmpty()
                        where subRent == null || subRent.RentDate.HasValue && subRent.RentDate.Value.Year == currentYear
                        group subRent by new { m.MonthNumber } into g
                        select new
                        {
                            RentMonth = g.Key.MonthNumber,
                            RentYear = currentYear,
                            RentCount = g.Count(r => r != null)
                        };

            var result = query.OrderBy(r => r.RentYear).ThenBy(r => r.RentMonth).ToList();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCountBooksByMonth()
        {
            try
            {
                int currentYear = DateTime.Now.Year;

                var months = Enumerable.Range(1, 12).Select(month => new { MonthNumber = month });

                var query = from m in months
                            join r in context.Rents on m.MonthNumber equals (r.RentDate.HasValue ? r.RentDate.Value.Month : 0) into rentGroup
                            from subRent in rentGroup.DefaultIfEmpty()
                            where subRent == null || subRent.RentDate.HasValue && subRent.RentDate.Value.Year == currentYear
                            join d in context.RentDetails on subRent?.RentId equals d.RentId into detailGroup
                            from subDetail in detailGroup.DefaultIfEmpty()
                            group subDetail by new { m.MonthNumber } into g
                            select new
                            {
                                RentMonth = g.Key.MonthNumber,
                                RentYear = currentYear,
                                BookCount = g.Count(d => d != null && d.BookId != null)
                            };

                var result =  query.OrderBy(r => r.RentYear).ThenBy(r => r.RentMonth).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    
    }
}

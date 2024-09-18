using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_librarymanagment.Data;
using api_librarymanagment.Models.Library;

namespace api_librarymanagment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CategoriesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await context.Categories.ToListAsync();
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(string id)
        {
            var category = await context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

    }
}

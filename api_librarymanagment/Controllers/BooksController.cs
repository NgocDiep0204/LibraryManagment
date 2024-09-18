using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_librarymanagment.Data;
using api_librarymanagment.Data.DTOs;
using api_librarymanagment.Models.Library;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.AspNetCore.Authorization;
using api_librarymanagment.Functions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace api_librarymanagment.Controllers
{
   // [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private static int pageSize = 10;
        public BooksController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/Books/GetAll
        [HttpGet("{pageNumber}")]
        public async Task<IActionResult> GetAll(int pageNumber = 1)
        {
            // Đảm bảo pageNumber là số nguyên dương
            if (pageNumber <= 0)
            {
                return BadRequest("Page number must be greater than 0.");
            }

            var query = await context.Books
                .Include(b => b.CategoryIdNavigation)
                .Include(c => c.AuthorIdNavigation)
                .Include(a => a.PublicatorIdNavigation)
                .Include(p => p.PositionIdNavigation)
                .AsNoTracking()
                .OrderBy(b => b.BookId)
                .ToListAsync();

            var pagedBooks = PagedList<Book>.Create(query, pageNumber, pageSize);
            return Ok(new
            {
                // query,
                pagedBooks,
                pagedBooks.TotalPages,
            });
        }


        // POST: api/Books/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookDTO book)
        {
            var newbook = new Book()
            {
                //BookId = Guid.NewGuid().ToString(),
                BookId = book.BookId,
                PublicatorId = book.PublicatorId,
                AuthorId = book.AuthorId,
                PositionId = book.PositionId,
                CategoryId = book.CategoryId,
                Name = book.Name,
                Language = book.Language,
                Quantity = book.Quantity,
                Image = book.Image
            };
            await context.Books.AddAsync(newbook);
            context.SaveChanges();
            return Ok(newbook);

        }

        // PUT: api/Books/Update/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] BookDTO book)
        {
            var bookUpdate = await context.Books.FirstOrDefaultAsync(b => b.BookId == id);
            if (bookUpdate == null)
            {
                return NotFound();
            }
            bookUpdate.PublicatorId = book.PublicatorId;
            bookUpdate.AuthorId = book.AuthorId;
            bookUpdate.PositionId = book.PositionId;
            bookUpdate.CategoryId = book.CategoryId;
            bookUpdate.Name = book.Name;
            bookUpdate.Language = book.Language;
            bookUpdate.Quantity = book.Quantity;
            bookUpdate.Image = book.Image;
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var book = await context.Books
                    .Include(b => b.CategoryIdNavigation)
                    .Include(c => c.AuthorIdNavigation)
                    .Include(a => a.PublicatorIdNavigation)
                    .Include(p => p.PositionIdNavigation)
                    .Where(b => b.BookId == id)
                    .ToListAsync();

            if (!book.Any())
            {
                return NotFound();
            }

            return Ok(book);
        }


        // GET: api/Books/GetById/5
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name, int pageNumber = 1)
        {
            var book = await context.Books
                           .Include(b => b.CategoryIdNavigation)
                           .Include(c => c.AuthorIdNavigation)
                           .Include(a => a.PublicatorIdNavigation)
                           .Include(p => p.PositionIdNavigation)
                           .Where(b => EF.Functions.Collate(b.Name, "SQL_Latin1_General_CP1_CI_AI").Contains(name))
                           .AsNoTracking()
                           .OrderBy(b => b.BookId)
                           .ToListAsync();
                           
            if (!book.Any())
            {
                return NotFound();
            }
            var pagedBooks = PagedList<Book>.Create(book, pageNumber, pageSize);

            return Ok(new
            {
                pagedBooks,
                pagedBooks.TotalPages,
            });
        }


        // GET: api/Books/GetById/5
        [HttpGet("{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            category = category.Trim();
            var books = await context.Books
                .Include(b => b.CategoryIdNavigation)
                .Include(c => c.AuthorIdNavigation)
                .Include(a => a.PublicatorIdNavigation)
                .Include(p => p.PositionIdNavigation)
                .Where(b => EF.Functions.Collate(b.CategoryIdNavigation.Name, "SQL_Latin1_General_CP1_CI_AI").Contains(category))
                .Select(b => new BookDTO
                {
                    BookId = b.BookId,
                    Name = b.Name,
                    Language = b.Language,
                    // Các trường dữ liệu khác cần thiết
                })
                .ToListAsync();
            if (!books.Any())
            {
                return NotFound(); // hoặc phản hồi HTTP thích hợp nếu không tìm thấy sách
            }

            return Ok(books);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_librarymanagment.Data;
using api_librarymanagment.Functions;
using api_librarymanagment.Models.Library;


namespace api_librarymanagment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private int pageSize = 10;
        public StudentsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet("{pageNumber}")]
        public async Task<IActionResult> GetAllStudents(int pageNumber = 1)
        {
            var students = await context.Students.AsNoTracking().ToListAsync();
            var pagedStudents = PagedList<Student>.Create(students, pageNumber, pageSize);
            return Ok( new
            {
                pagedStudents,
                pagedStudents.TotalPages
            });
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetStudentByName(string name, int pageNumber)
        {
            var students = await context.Students
                .Where(b => EF.Functions.Collate(b.Name, "SQL_Latin1_General_CP1_CI_AI").Contains(name))
                .AsNoTracking()
                .ToListAsync();
             var pagedStudents = PagedList<Student>.Create(students, pageNumber, pageSize);
            return Ok(new
            {
                pagedStudents,
                pagedStudents.TotalPages
            });
        }

        [HttpGet("{phone}")]
        public async Task<IActionResult> GetStudentByPhone(string phone)
        {
            var student = await context.Students
                .Where(b => b.PhoneNumber == phone)
                .ToListAsync();
            return Ok(student);
        }
    }
}

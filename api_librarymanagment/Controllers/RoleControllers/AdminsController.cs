using api_librarymanagment.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api_librarymanagment.Controllers.RoleControllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public AdminsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IEnumerable<string> GetAll()
        {
            return new List<string> { "value1", "value2" };
        }
    }
}

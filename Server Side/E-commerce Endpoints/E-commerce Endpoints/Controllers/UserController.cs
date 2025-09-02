using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Endpoints.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly appDbContext _context;
        public UserController(ILogger<UserController> logger , appDbContext context) { 
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            var records = _context.Users.ToList();
            if (records.Count > 0)
            {
                _logger.LogInformation("All users retrived from database");
                return Ok(records);
            }
            return BadRequest();
        }
    }
}

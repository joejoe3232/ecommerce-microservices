using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("{id:int}")]
        public IActionResult GetUser(int id)
        {
            return Ok(new { Id = id, Name = $"User {id}" });
        }
    }
}

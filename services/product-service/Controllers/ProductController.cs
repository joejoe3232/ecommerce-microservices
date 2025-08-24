using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet("{id:int}")]
        public IActionResult GetProduct(int id)
        {
            return Ok(new { Id = id, Name = $"Product {id}", Price = 100 + id });
        }
    }
}

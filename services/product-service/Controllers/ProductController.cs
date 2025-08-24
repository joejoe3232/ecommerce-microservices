using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Controllers
{
    /// <summary>
    /// 商品管理 API
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// 獲取指定商品信息
        /// </summary>
        /// <param name="id">商品ID</param>
        /// <returns>商品信息</returns>
        /// <response code="200">成功返回商品信息</response>
        /// <response code="404">商品不存在</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductResponse), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetProduct([Range(1, int.MaxValue)] int id)
        {
            if (id > 100)
            {
                return NotFound(new { Message = "商品不存在" });
            }
            
            return Ok(new ProductResponse 
            { 
                Id = id, 
                Name = $"Product {id}",
                Description = $"這是商品 {id} 的描述",
                Price = 100.00m + id,
                Stock = 50 + id,
                Category = "電子產品",
                CreatedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// 獲取所有商品列表
        /// </summary>
        /// <returns>商品列表</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ProductResponse>), 200)]
        public IActionResult GetAllProducts()
        {
            var products = Enumerable.Range(1, 5).Select(i => new ProductResponse
            {
                Id = i,
                Name = $"Product {i}",
                Description = $"這是商品 {i} 的描述",
                Price = 100.00m + i,
                Stock = 50 + i,
                Category = "電子產品",
                CreatedAt = DateTime.UtcNow.AddDays(-i)
            }).ToList();

            return Ok(products);
        }

        /// <summary>
        /// 創建新商品
        /// </summary>
        /// <param name="request">商品創建請求</param>
        /// <returns>創建的商品信息</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), 201)]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newProduct = new ProductResponse
            {
                Id = new Random().Next(1000, 9999),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                Category = request.Category,
                CreatedAt = DateTime.UtcNow
            };

            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
        }
    }

    /// <summary>
    /// 商品響應模型
    /// </summary>
    public class ProductResponse
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 商品價格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 庫存數量
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// 商品分類
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 創建商品請求模型
    /// </summary>
    public class CreateProductRequest
    {
        /// <summary>
        /// 商品名稱
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 商品描述
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 商品價格
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        /// <summary>
        /// 庫存數量
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        /// <summary>
        /// 商品分類
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;
    }
}

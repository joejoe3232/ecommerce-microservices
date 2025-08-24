using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UserService.Controllers
{
    /// <summary>
    /// 用戶管理 API
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// 獲取指定用戶信息
        /// </summary>
        /// <param name="id">用戶ID</param>
        /// <returns>用戶信息</returns>
        /// <response code="200">成功返回用戶信息</response>
        /// <response code="404">用戶不存在</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetUser([Range(1, int.MaxValue)] int id)
        {
            if (id > 100)
            {
                return NotFound(new { Message = "用戶不存在" });
            }
            
            return Ok(new UserResponse 
            { 
                Id = id, 
                Name = $"User {id}",
                Email = $"user{id}@example.com",
                CreatedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// 獲取所有用戶列表
        /// </summary>
        /// <returns>用戶列表</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserResponse>), 200)]
        public IActionResult GetAllUsers()
        {
            var users = Enumerable.Range(1, 5).Select(i => new UserResponse
            {
                Id = i,
                Name = $"User {i}",
                Email = $"user{i}@example.com",
                CreatedAt = DateTime.UtcNow.AddDays(-i)
            }).ToList();

            return Ok(users);
        }

        /// <summary>
        /// 創建新用戶
        /// </summary>
        /// <param name="request">用戶創建請求</param>
        /// <returns>創建的用戶信息</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), 201)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = new UserResponse
            {
                Id = new Random().Next(1000, 9999),
                Name = request.Name,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow
            };

            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }
    }

    /// <summary>
    /// 用戶響應模型
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 用戶郵箱
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 創建用戶請求模型
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// 用戶名稱
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 用戶郵箱
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}

using api.Data;
using api.DTO;
using api.Interaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ApplicationDbContext _applicationDbContext;

        public AccountController(IAccountService accountService, ApplicationDbContext applicationDbContext)
        {
            _accountService = accountService;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet("GetInfo")]
        public IActionResult GetInfo()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var res = _applicationDbContext.Users.FirstOrDefault(u => u.UserId == Convert.ToInt32(userId));

            return Ok(new { Role = res.Role, Id = res.UserId });
        }

        [HttpGet("GetUserInfo")]
        public IActionResult GetUserInfo()
        {
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Користувач не авторизований");

            var user = _applicationDbContext.Users.FirstOrDefault(u => u.UserId == Convert.ToInt32(userId));

            if (user == null)
                return NotFound("Користувача не знайдено");

            return Ok(new
            {
                Id = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt.ToString("yyyy-MM-dd") // або user.CreatedAt.ToShortDateString()
            });
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDTO request)
        {
            var respond = _accountService.Login(request);
            if (respond == null)
            {
                return Unauthorized("Невірний email або пароль");
            }
            return Ok(new { Token = respond });

        }

        [Authorize]
        [HttpGet("GetEmail")]
        public IActionResult GetEmail()
        {
            var userId = User.FindFirst("UserId")?.Value;

            return Ok("User is " + userId);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(long id)
        {
            var user = await _applicationDbContext.Users
                .AsNoTracking()
                .Where(u => u.UserId == id)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    Name = u.Name,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto request)
        {
            var respond = _accountService.Register(request);
            if (respond == null)
            {
                return BadRequest("Користувач з таким email вже існує.");
            }


            return Ok(new { Token = respond });
        }

    }
}

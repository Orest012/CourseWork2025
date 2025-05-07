using api.Data;
using api.DTO;
using api.Interaces;
using api.Models;
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


        //[HttpPost("register")]
        //public IActionResult Register([FromBody] UserRegisterDto request)
        //{
        //    var respond = _accountService.Register(request);
        //    if (respond == null)
        //    {
        //        return BadRequest("Користувач з таким email вже існує.");
        //    }
        //    return Ok(new { Token = respond });
        //}

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto request)
        {
            // Перевірка, чи користувач з таким email вже існує
            var existingUser = _applicationDbContext.Users.Any(u => u.Email == request.Email);
            if (existingUser)
            {
                return BadRequest("Користувач з таким email вже існує.");
            }

            // Хешування пароля
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Отримання наступного UserId
            var nextUserId = (_applicationDbContext.Users.Max(u => (long?)u.UserId) ?? 0) + 1;

            // Створення нового користувача
            var newUser = new User
            {
                UserId = nextUserId,
                Email = request.Email,
                Name = request.Name,
                Password = hashedPassword,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                Role = "User"
            };

            _applicationDbContext.Users.Add(newUser);

            // Отримання наступного PaymentId
            var nextPaymentId = (_applicationDbContext.Payments.Max(p => (long?)p.PaymentId) ?? 0) + 1;

            var payment = new Payment
            {
                PaymentId = nextPaymentId,
                UserId = newUser.UserId,
                PaymentDate = DateTime.UtcNow,
                Amount = 0,
                PaymentMethod = "Не вказано"
            };

            _applicationDbContext.Payments.Add(payment);

            // Якщо роль — Admin, додаємо в Organizers
            if (newUser.Role == "Admin")
            {
                var nextOrganizerId = (_applicationDbContext.Organizers.Max(o => (long?)o.OrganizerId) ?? 0) + 1;

                var organizer = new Organizer
                {
                    OrganizerId = nextOrganizerId,
                    UserId = newUser.UserId
                };

                _applicationDbContext.Organizers.Add(organizer);
            }

            _applicationDbContext.SaveChanges();

            return Ok(new { message = "Користувач зареєстрований успішно." });
        }



    }
}

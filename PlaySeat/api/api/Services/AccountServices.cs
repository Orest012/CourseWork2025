using api.Data;
using api.DTO;
using api.Interaces;
using api.Models;
using Org.BouncyCastle.Crypto.Generators;

namespace api.Services
{
    public class AccountService : IAccountService
    {
        private readonly Data.ApplicationDbContext _appDbContext;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _config;

        public AccountService(Data.ApplicationDbContext appDbContext, TokenService tokenService, IConfiguration config)
        {
            _appDbContext = appDbContext;
            _tokenService = tokenService;
            _config = config;
        }

        public string Register(UserRegisterDto request)
        {
            var existingUser = _appDbContext.Users.Any(u => u.Email == request.Email);
            if (existingUser)
            {
                return null;
            }
            request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            
            User user = new User
            {
                Email = request.Email,
                Password = request.Password,
                Name = request.Name,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                Role = "User"
            };


            _appDbContext.Add(user);
            _appDbContext.SaveChanges();
            var token = _tokenService.GenerateToken(user);

            return token;

        }

        public string Login(UserLoginDTO request)
        {
            var user = _appDbContext.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return null;
            }
            var token = _tokenService.GenerateToken(user);
            return token;
        }

    }
}

using api.DTO;

namespace api.Interaces
{
    public interface IAccountService
    {
        string Login(UserLoginDTO request);
        string Register(UserRegisterDto request);
    }
}

using Disney.Auth.Request;
using Disney.Auth.Response;

namespace Disney.Services.Interfaces
{
    public interface IUserService
    {
        UserResponse Register(RegisterRequest userRequest, string password);
        UserResponse Login(LoginRequest userLogin);
        string GetToken(UserResponse userResponse);
    }
}

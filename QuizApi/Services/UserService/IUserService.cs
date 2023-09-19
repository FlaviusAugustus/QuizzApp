namespace QuizApi.Services.UserService;

public interface IUserService
{
    Task<string> RegisterAsync(RegisterModel registerModel);
    Task<AuthModel> GetTokenAsync(TokenRequestModel requestModel);
    Task<string> AddToRoleAsync(AddRoleModel roleModel);
}

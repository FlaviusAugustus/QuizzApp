using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuizApi.Constants;
using QuizApi.Settings;

namespace QuizApi.Services.UserService;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly JWT _jwtConfig;

    public UserService(UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager, IOptions<JWT> jwt) =>
        (_userManager, _roleManager, _jwtConfig) = (userManager, roleManager, jwt.Value);

    public async Task<string> AddToRoleAsync(AddRoleModel roleModel)
    {
        var user = await _userManager.FindByNameAsync(roleModel.UserName);
        if (user is null)
        {
            return $"User {user.UserName} doesn't exist";
        }
        if (await _userManager.CheckPasswordAsync(user, roleModel.Password))
        {
            var roleLower = roleModel.Role.ToLower();
            var roleExists = Enum.GetValues(typeof(Roles))
                .Cast<Roles>()
                .Any(role => roleLower == role.ToString().ToLower());
            if (roleExists)
            {
                var role = Enum.GetValues<Roles>().First(role => role.ToString().ToLower() == roleLower);
                await _userManager.AddToRoleAsync(user, role.ToString());
                return $"Added {roleModel.Role} to user {roleModel.UserName}";
            }
        }
        return $"Invalid credentials for user {roleModel.UserName}";
    }

    public async Task<AuthModel> GetTokenAsync(TokenRequestModel requestModel)
    {
        var authModel = new AuthModel();
        var user = await _userManager.FindByEmailAsync(requestModel.Email);
        if (user is null)
        {
            authModel.IsAuthenticated = false;
            authModel.Message = $"No accounts with Email: {authModel.Email}";
            return authModel;
        }

        if (await _userManager.CheckPasswordAsync(user, requestModel.Password))
        {
            authModel.IsAuthenticated = true;
            var securityToken = await CreateJwtToken(user);
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            authModel.Roles = roles.ToList();
            return authModel;
        }
        authModel.IsAuthenticated = false;
        authModel.Message = "Invalid Credentials";
        return authModel;
    }

    private async Task<JwtSecurityToken> CreateJwtToken(User user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(j => new Claim("roles", j)).ToList();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id.ToString())
        }.Union(userClaims).Union(roleClaims);

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtConfig.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }

    public async Task<string> RegisterAsync(RegisterModel registerModel)
    {
        var user = new User
        {
            UserName = registerModel.UserName,
            Email = registerModel.Email,
            FirstName = registerModel.FirstName,
            Password = registerModel.Password
        };
        if (!await IsUserDataAvailable(user)) 
        {
            return "Invalid credentials";
        }            
        var result = await _userManager.CreateAsync(user, registerModel.Password);
        if (result.Succeeded)
        {
            //await _userManager.AddToRoleAsync(user, Roles.User.ToString());
            return $"Account with username: {user.UserName} has been created";
        }
        return "Failed";
    }

    private async Task<bool> IsUserDataAvailable(User user)
    {
        var sameEmail = await  _userManager.FindByEmailAsync(user.Email);
        return sameEmail is null;
    }
}
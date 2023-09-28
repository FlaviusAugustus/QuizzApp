using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuizApi.Constants;
using QuizApi.Services.DateTimeProvider;
using QuizApi.Settings;

namespace QuizApi.Services.UserService;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JWT _jwtConfig;

    public UserService(UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager, IOptions<JWT> jwt,
        IDateTimeProvider dateTimeProvider) =>
        (_userManager, _roleManager, _jwtConfig, _dateTimeProvider) = (userManager, roleManager, jwt.Value, dateTimeProvider);

    public async Task<Result<Unit>> AddToRoleAsync(AddRoleModel roleModel)
    {
        var user = await _userManager.FindByNameAsync(roleModel.UserName);
        if (user is null)
        {
            var error = new ArgumentException($"User {roleModel.UserName} doesn't exist");
            return new Result<Unit>(error);
        }
        if (await _userManager.CheckPasswordAsync(user, roleModel.Password))
        {
            return await AddUserToRole(user, roleModel.Role); 
        }
        var e = new ArgumentException($"Invalid credentials for user {roleModel.UserName}");
        return new Result<Unit>(e);
    }

    private async Task<Result<Unit>> AddUserToRole(User user, string requestedRole)
    {
        if (Enum.TryParse<Roles>(requestedRole, out var role))
        {
            await _userManager.AddToRoleAsync(user, role.ToString());
            return new Result<Unit>();
        }

        var invalidRoleException = new ArgumentException($"No existing role: {role}");
        return new Result<Unit>(invalidRoleException);
    }

    public async Task<Result<AuthModel>> GetTokenAsync(TokenRequestModel requestModel)
    {
        var user = await _userManager.FindByEmailAsync(requestModel.Email);
        if (user is null)
        {
            var noAccountError = new ArgumentException($"No accounts with Email: {requestModel.Email}");
            return new Result<AuthModel>(noAccountError);
        }

        if (await _userManager.CheckPasswordAsync(user, requestModel.Password))
        {
            var token = await CreateTokenForUser(user);
            return new Result<AuthModel>(token);
        }
        
        var invalidCredentialsError = new ArgumentException($"Invalid Credentials for user {user.UserName}");
        return new Result<AuthModel>(invalidCredentialsError);
    }

    private async Task<AuthModel> CreateTokenForUser(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return new AuthModel
        {
            IsAuthenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(await CreateJwtToken(user)),
            Email = user.Email,
            UserName = user.UserName,
            Roles = roles.ToList(),
        };
    }

    private async Task<JwtSecurityToken> CreateJwtToken(User user)
    {
        var claims = await CreateClaims(user);
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);
        var expires = _dateTimeProvider.GetCurrentTime().AddMinutes(_jwtConfig.DurationInMinutes);

        return new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials);
    }

    private async Task<IEnumerable<Claim>> CreateClaims(User user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(j => new Claim("roles", j));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        
        return claims.Union(userClaims).Union(roleClaims);
    }

    public async Task<Result<User>> RegisterAsync(RegisterModel registerModel)
    {
        var user = new User
        {
            UserName = registerModel.UserName,
            Email = registerModel.Email,
            FirstName = registerModel.FirstName,
            Password = registerModel.Password
        };
        if (await IsUserDataTaken(user))
        {
            var credentialsError = new ArgumentException("Invalid Credentials");
            return new Result<User>(credentialsError);
        }
        
        var result = await _userManager.CreateAsync(user, registerModel.Password);
        if (result.Succeeded)
        {
            return new Result<User>(user);
        }
        
        var creationError = new ArgumentException($"Couldn't create user {user.UserName}");
        return new Result<User>(creationError);
    }

    private async Task<bool> IsUserDataTaken(User user)
    {
        var sameEmail = await  _userManager.FindByEmailAsync(user.Email);
        return sameEmail is not null;
    }
}
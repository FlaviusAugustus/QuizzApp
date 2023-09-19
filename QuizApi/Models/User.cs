using Microsoft.AspNetCore.Identity;

namespace QuizApi;

public class User : IdentityUser<Guid>, IEntity
{
    public string FirstName { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
}
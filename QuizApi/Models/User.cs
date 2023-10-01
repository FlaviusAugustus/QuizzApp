using Microsoft.AspNetCore.Identity;

namespace QuizApi;

public class User : IdentityUser<Guid>, IEntity
{
    public DateTime CreatedAt { get; set; }
}
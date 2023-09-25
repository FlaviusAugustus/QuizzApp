using Microsoft.EntityFrameworkCore;

namespace QuizApi.Repository;

public class UserRepository : GenericRepository<User>, IRepositoryUser
{
    public UserRepository(QuizContext context) : base(context) {}

    public async Task<User?> GetByUserName(string userName) =>
        await _context.Set<User>().SingleOrDefaultAsync(u => u.UserName == userName)!;

    public async Task<User?> GetByEmail(string email) =>
        await _context.Set<User>().SingleOrDefaultAsync(u => u.Email == email)!;
}
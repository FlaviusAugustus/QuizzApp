namespace QuizApi.Repository;

public class UserRepository : GenericRepository<User>, IRepositoryUser
{
    public UserRepository(QuizContext context) : base(context) {}

    public User GetByUserName(string userName) =>
        _context.Set<User>().SingleOrDefault(u => u.UserName == userName)!;

    public User GetByEmail(string email) =>
        _context.Set<User>().SingleOrDefault(u => u.Email == email)!;
}
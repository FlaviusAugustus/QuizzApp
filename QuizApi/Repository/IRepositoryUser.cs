namespace QuizApi.Repository;

public interface IRepositoryUser : IGenericRepository<User>
{
    public Task<User?> GetByUserName(string userName);
    public Task<User?> GetByEmail(string email);
}
namespace QuizApi.Repository;

public interface IRepositoryUser : IGenericRepository<User>
{
    public User GetByUserName(string userName);
    public User GetByEmail(string email);
}
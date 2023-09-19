using System.Linq.Expressions;

namespace QuizApi.Repository;

public interface IGenericRepository<TEntity> where TEntity : class, IEntity
{
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    void RemoveById(Guid id);
    void Save();
    TEntity GetById(Guid id);
    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
}
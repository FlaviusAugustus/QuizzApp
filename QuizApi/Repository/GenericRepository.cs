using System.Linq.Expressions;

namespace QuizApi.Repository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
{
    protected readonly QuizContext _context;

    protected GenericRepository(QuizContext context) =>
        _context = context;

    public void Add(TEntity entity) =>
        _context.Set<TEntity>().Add(entity);

    public void Update(TEntity entity) =>
        _context.Set<TEntity>().Update(entity);

    public void Remove(TEntity entity) =>
        _context.Set<TEntity>().Remove(entity);

    public void Save() =>
        _context.SaveChanges();

    public void RemoveById(Guid id)
    {
        var entity = _context.Set<TEntity>().SingleOrDefault(e => e.Id == id);
        if (entity is not null)
        {
            _context.Remove(entity);
        }
    }

    public TEntity GetById(Guid id) =>
        _context.Set<TEntity>().SingleOrDefault(e => e.Id == id)!;

    public IEnumerable<TEntity> GetAll() =>
        _context.Set<TEntity>().AsEnumerable();

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression) =>
        _context.Set<TEntity>().Where(expression);
}
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

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

    public async Task SaveAsync() =>
        await _context.SaveChangesAsync();

    public async Task RemoveByIdAsync(Guid id)
    {
        var entity = await _context.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == id);
        if (entity is not null)
        {
            _context.Remove(entity);
        }
    }
    
    public async Task<TEntity?> GetByIdAsync(Guid id) =>
        await _context.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<TEntity>> GetAllAsync() =>
        await _context.Set<TEntity>().ToListAsync();

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression) =>
        await _context.Set<TEntity>().Where(expression).ToListAsync();
}
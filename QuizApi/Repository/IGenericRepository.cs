﻿using System.Linq.Expressions;

namespace QuizApi.Repository;

public interface IGenericRepository<TEntity> where TEntity : class, IEntity
{
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task RemoveByIdAsync(Guid id);
    Task SaveAsync();
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression);
}
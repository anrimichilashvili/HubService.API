using HubService.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HubService.Infrastructure.Implementation
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext context;
        public Repository(ApplicationDbContext context) => this.context = context;




        public virtual async Task<TEntity?> ReadAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }
        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default, params string[] includes)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();
            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }
            cancellationToken.ThrowIfCancellationRequested();
            return await query.FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression,
         CancellationToken cancellationToken = default, params string[] includes)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();
            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }
            cancellationToken.ThrowIfCancellationRequested();
            return await query.ToListAsync();

        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var result = await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
        public virtual async Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            context.Set<TEntity>().Update(entity);
            return await context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<int> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var item = await this.ReadAsync(id);
            if (item is null) return 0;
            context.Set<TEntity>().Remove(item);
            return await context.SaveChangesAsync(cancellationToken);
        }

    }
}

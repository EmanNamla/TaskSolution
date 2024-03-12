using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskToRepoteqCompany.BLL.Repositoies.Abstraction;
using TaskToRepoteqCompany.DAL.Contexts;
using TaskToRepoteqCompany.DAL.Models;

namespace TaskToRepoteqCompany.BLL.Repositoies.Implementation
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly EcommerceDbContext dbContext;

        public GenericRepository(EcommerceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if (typeof(TEntity) == typeof(Order))
            {
                return (IEnumerable<TEntity>)await dbContext.Orders
                    .Include(m => m.OrderProducts)
                        .ThenInclude(op => op.Product)
                    .ToListAsync();
            }

            return await dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            if (typeof(TEntity) == typeof(Order))
            {
                return await dbContext.Orders
                    .Include(m => m.OrderProducts)

                        .ThenInclude(am => am.Product)
                    .FirstOrDefaultAsync(m => m.Id == id) as TEntity;
            }
            return await dbContext.Set<TEntity>().FindAsync(id);
        }


        public async Task AddAsync(TEntity item)
        { await dbContext.Set<TEntity>().AddAsync(item); }

        public void Delete(TEntity item)
        {
            dbContext.Set<TEntity>().Remove(item);
        }

        public void Update(TEntity item)
        {
            dbContext.Set<TEntity>().Update(item);
        }

        public void Detach(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = dbContext.Entry(entity);
            if (entry != null)
            {
                entry.State = EntityState.Detached;
            }
        }

        public Task<int> GetCountAsync()
        {
            return dbContext.Set<TEntity>().CountAsync();
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TaskToRepoteqCompany.BLL.Repositoies.Abstraction
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);


        Task AddAsync(TEntity item);
        void Update(TEntity item);

        void Delete(TEntity item);

        void Detach(TEntity entity);
        Task<int> GetCountAsync();

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);





    }
}

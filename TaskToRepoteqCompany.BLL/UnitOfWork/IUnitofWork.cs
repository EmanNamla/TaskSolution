using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskToRepoteqCompany.BLL.Repositoies.Abstraction;

namespace TaskToRepoteqCompany.BLL.UnitOfWork
{
    public interface IUnitofWork : IAsyncDisposable
    {
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

        Task<int> CompleteAsync();
    
    }
}

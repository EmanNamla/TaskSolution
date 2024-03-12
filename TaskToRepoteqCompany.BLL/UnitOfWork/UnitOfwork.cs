using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskToRepoteqCompany.BLL.Repositoies.Abstraction;
using TaskToRepoteqCompany.BLL.Repositoies.Implementation;
using TaskToRepoteqCompany.DAL.Contexts;

namespace TaskToRepoteqCompany.BLL.UnitOfWork
{
    public class UnitOfwork:IUnitofWork
    {
        private readonly EcommerceDbContext dbContext;
        private Hashtable _Repository;

        public UnitOfwork(EcommerceDbContext dbContext)
        {
            this.dbContext = dbContext;
            _Repository = new Hashtable();
        }
        public async Task<int> CompleteAsync()
           => await dbContext.SaveChangesAsync();


        public async ValueTask DisposeAsync()
        {
            await dbContext.DisposeAsync();
        }

        IGenericRepository<TEntity> IUnitofWork.Repository<TEntity>()
        {
            var type = typeof(TEntity).Name;

            if (!_Repository.Contains(type))
            {
                var Repository = new GenericRepository<TEntity>(dbContext);
                _Repository.Add(type, Repository);
            }
            return _Repository[type] as IGenericRepository<TEntity>;
        }
    }
}
